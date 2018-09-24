using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Buildalyzer;
using NScanRoot.App;
using NScanRoot.ForFun;
using NScanRoot.Xml;

namespace NScanRoot.CompositionRoot
{
  internal class ProjectPaths
  {
    private readonly IEnumerable<string> _projectFilePaths;
    private readonly ISupport _support;

    public ProjectPaths(IEnumerable<string> projectFilePaths, ISupport support)
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
      var xmlProject = ProjectPaths.DeserializeProjectFile(projectFilePath);
      xmlProject.AbsolutePath = projectFilePath;
      ProjectPaths.NormalizeProjectDependencyPaths(projectFilePath, xmlProject);
      return xmlProject;
    }

    public List<XmlProject> LoadXmlProjects()
    {
      //todo what about exceptions?
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

    public static ProjectPaths From(string solutionFilePath, ConsoleSupport consoleSupport)
    {
      var analyzerManager = new AnalyzerManager(solutionFilePath);
      var projectFilePaths = analyzerManager.Projects.Select(p => p.Value.ProjectFile.Path).ToList();
      var paths = new ProjectPaths(projectFilePaths, consoleSupport);
      return paths;
    }
  }
}