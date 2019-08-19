using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AtmaFileSystem;
using Buildalyzer;
using Functional.Maybe;
using Functional.Maybe.Just;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Lib;
using NScan.SharedKernel.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
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

    public List<XmlProject> LoadXmlProjects()
    {
      var xmlProjects = _projectFilePaths.Select(LoadXmlProjectFromPath())
        .Where(maybeProject => maybeProject.HasValue)
        .Select(maybeProject => maybeProject.Value).ToList();
      return xmlProjects;
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

      SourceCodeFilePaths.LoadFilesInto(xmlProjectData);
      return xmlProjectData.ToXmlProject();
    }

    private static XmlProjectDataAccess DeserializeProjectData(AbsoluteFilePath projectFilePath)
    {
      var xmlProjectDataAccess = new XmlProjectDataAccess(DeserializeProjectFile(projectFilePath));
      xmlProjectDataAccess.SetAbsolutePath(projectFilePath);
      xmlProjectDataAccess.NormalizeProjectDependencyPaths(projectFilePath);
      return xmlProjectDataAccess;
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
  }
}