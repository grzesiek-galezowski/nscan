using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AtmaFileSystem;
using Buildalyzer;
using Functional.Maybe;
using Functional.Maybe.Just;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingCSharpSourceCode;
using TddXt.NScan.ReadingSolution.Lib;
using TddXt.NScan.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace TddXt.NScan.ReadingSolution.Adapters
{
  public class ProjectPaths
  {
    private readonly IEnumerable<AbsoluteFilePath> _projectFilePaths;
    private readonly INScanSupport _support;

    public ProjectPaths(IEnumerable<AbsoluteFilePath> projectFilePaths, INScanSupport support)
    {
      _projectFilePaths = projectFilePaths;
      _support = support;
    }

    private static XmlProject DeserializeProjectFile(AbsoluteFilePath projectFilePath)
    {
      var serializer = new XmlSerializer(typeof(XmlProject));
      XmlProject result;
      using (var fileStream = new FileStream(projectFilePath.ToString(), FileMode.Open))
      {
        result = (XmlProject) serializer.Deserialize(fileStream);
      }

      return result;
    }

    private static XmlProject LoadXmlProject(AbsoluteFilePath projectFilePath)
    {
      var xmlProjectData = DeserializeProjectData(projectFilePath);
      xmlProjectData.SetAbsolutePath(projectFilePath);
      xmlProjectData.NormalizeProjectDependencyPaths(projectFilePath);
      xmlProjectData.NormalizeProjectAssemblyName();
      xmlProjectData.NormalizeProjectRootNamespace();
      LoadFilesInto(xmlProjectData);
      return xmlProjectData.ToXmlProject();
    }

    private static XmlProjectDataAccess DeserializeProjectData(AbsoluteFilePath projectFilePath)
    {
      return new XmlProjectDataAccess(DeserializeProjectFile(projectFilePath));
    }


    public List<XmlProject> LoadXmlProjects()
    {
      var xmlProjects = _projectFilePaths.Select(LoadXmlProjectFromPath())
        .Where(maybeProject => maybeProject.HasValue)
        .Select(maybeProject => maybeProject.Value).ToList();
      return xmlProjects;
    }

    private Func<AbsoluteFilePath, Maybe<XmlProject>> LoadXmlProjectFromPath()
    {
      return path =>
      {
        try
        {
          return LoadXmlProject(path).Just();
        }
        catch (InvalidOperationException e)
        {
          _support.SkippingProjectBecauseOfError(e, path);
          return Maybe<XmlProject>.Nothing;
        }
      };
    }

    public static ProjectPaths From(string solutionFilePath, INScanSupport consoleSupport)
    {
      var analyzerManager = new AnalyzerManager(solutionFilePath);
      var projectFilePaths = analyzerManager.Projects
        .Select(p => p.Value.ProjectFile.Path)
        .Select(AbsoluteFilePath).ToList();
      var paths = new ProjectPaths(projectFilePaths, consoleSupport);
      return paths;
    }

    private static void LoadFilesInto(XmlProjectDataAccess projectAccess)
    {
      var projectDirectory = projectAccess.GetParentDirectoryName();

      var syntaxTrees = SourceCodeFilesIn(projectDirectory).Select(CSharpFileSyntaxTree.ParseFile).ToArray();

      var classDeclarationSignatures
        = CSharpFileSyntaxTree.GetClassDeclarationSignaturesFromFiles(syntaxTrees);

      foreach (var dotNetProject 
        in syntaxTrees.Select(tree => CreateXmlSourceCodeFile(projectAccess, projectDirectory, tree, classDeclarationSignatures)))
      {
        projectAccess.AddFile(dotNetProject);
      }
    }

    private static XmlSourceCodeFile CreateXmlSourceCodeFile(XmlProjectDataAccess projectAccess, AbsoluteDirectoryPath projectDirectory, CSharpFileSyntaxTree syntaxTree, Dictionary<string, ClassDeclarationInfo> classDeclarationSignatures)
    {
      return new XmlSourceCodeFile(
        GetPathRelativeTo(projectDirectory, syntaxTree.FilePath),
        syntaxTree.GetAllUniqueNamespaces().ToList(),
        projectAccess.RootNamespace(), 
        projectAccess.DetermineAssemblyName(), 
        syntaxTree.GetAllUsingsFrom(classDeclarationSignatures)
      );
    }

    private static IEnumerable<AbsoluteFilePath> SourceCodeFilesIn(AbsoluteDirectoryPath projectDirectory)
    {
      return Directory.EnumerateFiles(
          projectDirectory.ToString(), "*.cs", SearchOption.AllDirectories)
        .Select(f => AbsoluteFilePath(f))
        .Where(IsNotInDirectory(projectDirectory, "obj"));
    }

    private static Func<AbsoluteFilePath, bool> IsNotInDirectory(AbsoluteDirectoryPath projectDirectory, string dirName)
    {
      return f => !GetPathRelativeTo(projectDirectory, f).StartsWith(dirName + Path.DirectorySeparatorChar);
    }

    private static string GetPathRelativeTo(AbsoluteDirectoryPath projectDirectory, AbsoluteFilePath file)
    {
      return file.ToString() /* bug */.Replace(projectDirectory.ToString() /* bug */ + Path.DirectorySeparatorChar, "");
    }
  }
}