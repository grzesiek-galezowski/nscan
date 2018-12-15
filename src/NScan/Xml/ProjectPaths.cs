using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Buildalyzer;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using TddXt.NScan.Lib;

namespace TddXt.NScan.Xml
{
  public class ProjectPaths
  {
    private readonly IEnumerable<string> _projectFilePaths;
    private readonly INScanSupport _support;

    public ProjectPaths(IEnumerable<string> projectFilePaths, INScanSupport support)
    {
      _projectFilePaths = projectFilePaths;
      _support = support;
    }

    private static void NormalizeProjectDependencyPaths(string projectFileAbsolutePath, XmlProject xmlProjectReferences)
    {
      foreach (var projectReference in CsharpWorkspaceModel.ProjectReferences(xmlProjectReferences))
      {
        projectReference.Include =
          Path.GetFullPath(Path.Combine(Path.GetDirectoryName(projectFileAbsolutePath), projectReference.Include));
      }
    }

    public static XmlProject DeserializeProjectFile(string projectFilePath)
    {
      var serializer = new XmlSerializer(typeof(XmlProject));
      XmlProject result;
      using (var fileStream = new FileStream(projectFilePath, FileMode.Open))
      {
        result = (XmlProject) serializer.Deserialize(fileStream);
      }

      return result;
    }

    public static XmlProject LoadXmlProject(string projectFilePath)
    {
      var xmlProject = DeserializeProjectFile(projectFilePath);
      xmlProject.AbsolutePath = projectFilePath;
      NormalizeProjectDependencyPaths(projectFilePath, xmlProject);
      NormalizeProjectAssemblyName(xmlProject);
      NormalizeProjectRootNamespace(xmlProject);
      LoadFilesInto(xmlProject);
      return xmlProject;
    }

      private static void NormalizeProjectRootNamespace(XmlProject xmlProject)
      {
          if (xmlProject.PropertyGroups.All(g => g.RootNamespace == null))
          {
              xmlProject.PropertyGroups.First().RootNamespace
                  = Path.GetFileNameWithoutExtension(
                      Path.GetFileName(xmlProject.AbsolutePath));
          }
      }


      public List<XmlProject> LoadXmlProjects()
    {
      var xmlProjects = _projectFilePaths.Select(path =>
      {
        try
        {
          return Maybe.Just(LoadXmlProject(path));
        }
        catch (InvalidOperationException e)
        {
          _support.SkippingProjectBecauseOfError(e, path);
          return Maybe.Nothing<XmlProject>();
        }
      }).Where(o => o.HasValue).Select(o => o.Value()).ToList();
      return xmlProjects;
    }

    public static ProjectPaths From(string solutionFilePath, INScanSupport consoleSupport)
    {
      var analyzerManager = new AnalyzerManager(solutionFilePath);
      var projectFilePaths = analyzerManager.Projects.Select(p => p.Value.ProjectFile.Path).ToList();
      var paths = new ProjectPaths(projectFilePaths, consoleSupport);
      return paths;
    }

    private static void LoadFilesInto(XmlProject xmlProject)
    {
      var projectDirectory = Path.GetDirectoryName(xmlProject.AbsolutePath);
      foreach (var file in Directory.EnumerateFiles(projectDirectory, "*.cs", SearchOption.AllDirectories)
        .Where(IsNotInFolder(projectDirectory, "obj")))
      {
        var fileRelativeToProjectRoot = GetPathRelativeTo(projectDirectory, file);
        var declaredNamespace = CSharpSyntax.GetAllUniqueNamespacesFromFile(file).FirstOrDefault();
        xmlProject.SourceCodeFiles.Add(new XmlSourceCodeFile(
          fileRelativeToProjectRoot,
          declaredNamespace, //bug multiple namespaces not supported yet
          xmlProject.PropertyGroups.First().RootNamespace, 
          xmlProject.PropertyGroups.First().AssemblyName));
      }
    }

    private static Func<string, bool> IsNotInFolder(string projectDirectory, string dirName)
    {
      return f => !GetPathRelativeTo(projectDirectory, f).StartsWith(dirName + Path.DirectorySeparatorChar);
    }

    private static string GetPathRelativeTo(string projectDirectory, string file)
    {
      return file.Replace(projectDirectory + Path.DirectorySeparatorChar, "");
    }

    private static void NormalizeProjectAssemblyName(XmlProject xmlProject)
    {
      if (xmlProject.PropertyGroups.All(g => g.AssemblyName == null))
      {
        xmlProject.PropertyGroups.First().AssemblyName
          = Path.GetFileNameWithoutExtension(
            Path.GetFileName(xmlProject.AbsolutePath));
      }
    }
  }

  public static class CSharpSyntax
  {
    public static IEnumerable<string> GetAllUniqueNamespacesFrom(string fileText)
    {
      var tree = CSharpSyntaxTree.ParseText(fileText);
      return new HashSet<string>(tree.GetCompilationUnitRoot().Members.Where(MemberIsNamespace())
        .Cast<NamespaceDeclarationSyntax>().Select(NamespaceName()));
    }

    private static Func<NamespaceDeclarationSyntax, string> NamespaceName()
    {
      return ns => ns.Name.ToString();
    }

    private static Func<MemberDeclarationSyntax, bool> MemberIsNamespace()
    {
      return m => m is NamespaceDeclarationSyntax;
    }

    public static IEnumerable<string> GetAllUniqueNamespacesFromFile(string file)
    {
      return CSharpSyntax.GetAllUniqueNamespacesFrom(File.ReadAllText(file));
    }
  }
}