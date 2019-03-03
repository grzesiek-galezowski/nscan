using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Buildalyzer;
using Functional.Maybe.Just;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingCSharpSourceCode;
using TddXt.NScan.ReadingSolution.Lib;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.ReadingSolution.Adapters
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

    private static void NormalizeProjectDependencyPaths(string projectFileAbsolutePath, IXmlProjectDataAccess xmlProjectDataAccess)
    {
      foreach (var projectReference in xmlProjectDataAccess.ProjectReferences())
      {
        projectReference.Include =
          Path.GetFullPath(Path.Combine(Path.GetDirectoryName(projectFileAbsolutePath), projectReference.Include));
      }
    }

    private static XmlProject DeserializeProjectFile(string projectFilePath)
    {
      var serializer = new XmlSerializer(typeof(XmlProject));
      XmlProject result;
      using (var fileStream = new FileStream(projectFilePath, FileMode.Open))
      {
        result = (XmlProject) serializer.Deserialize(fileStream);
      }

      return result;
    }

    private static XmlProject LoadXmlProject(string projectFilePath)
    {
      //TODO use dataaccess in more places
      var xmlProject = DeserializeProjectFile(projectFilePath);
      xmlProject.AbsolutePath = projectFilePath;
      NormalizeProjectDependencyPaths(projectFilePath, new XmlProjectDataAccess(xmlProject));
      NormalizeProjectAssemblyName(xmlProject);
      NormalizeProjectRootNamespace(xmlProject);
      LoadFilesInto(new XmlProjectDataAccess(xmlProject));
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
          return LoadXmlProject(path).Just();
        }
        catch (InvalidOperationException e)
        {
          _support.SkippingProjectBecauseOfError(e, path);
          return Functional.Maybe.Maybe<XmlProject>.Nothing;
        }
      }).Where(o => o.HasValue).Select(o => o.Value).ToList();
      return xmlProjects;
    }

    public static ProjectPaths From(string solutionFilePath, INScanSupport consoleSupport)
    {
      var analyzerManager = new AnalyzerManager(solutionFilePath);
      var projectFilePaths = analyzerManager.Projects.Select(p => p.Value.ProjectFile.Path).ToList();
      var paths = new ProjectPaths(projectFilePaths, consoleSupport);
      return paths;
    }

    private static void LoadFilesInto(XmlProjectDataAccess projectAccess)
    {
      var projectDirectory = projectAccess.GetParentDirectoryName();
      var sourceCodeFilesInProject = SourceCodeFilesIn(projectDirectory);

      var syntaxTrees = sourceCodeFilesInProject.Select(CSharpFileSyntaxTree.ParseFile).ToArray();

      var classDeclarationSignatures
        = CSharpFileSyntaxTree.GetClassDeclarationSignaturesFromFiles(syntaxTrees);

      foreach (var dotNetProject 
        in syntaxTrees.Select(t => CreateXmlSourceCodeFile(projectAccess, projectDirectory, t, classDeclarationSignatures)))
      {
        projectAccess.AddFile(dotNetProject);
      }
    }

    private static XmlSourceCodeFile CreateXmlSourceCodeFile(XmlProjectDataAccess projectAccess, string projectDirectory, CSharpFileSyntaxTree syntaxTree, Dictionary<string, ClassDeclarationInfo> classDeclarationSignatures)
    {
      return new XmlSourceCodeFile(
        GetPathRelativeTo(projectDirectory, syntaxTree.FilePath),
        syntaxTree.GetAllUniqueNamespaces().ToList(),
        projectAccess.RootNamespace(), 
        projectAccess.DetermineAssemblyName(), 
        syntaxTree.GetAllUsingsFrom(classDeclarationSignatures)
      );
    }

    private static IEnumerable<string> SourceCodeFilesIn(string projectDirectory)
    {
      return Directory.EnumerateFiles(
          projectDirectory, "*.cs", SearchOption.AllDirectories)
        .Where(IsNotInDirectory(projectDirectory, "obj"));
    }

    private static Func<string, bool> IsNotInDirectory(string projectDirectory, string dirName)
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
}