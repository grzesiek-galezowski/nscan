using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AtmaFileSystem;
using Buildalyzer;
using Core.Maybe;
using Core.NullableReferenceTypesExtensions;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

public class ProjectPaths
{
  private readonly IEnumerable<AbsoluteFilePath> _projectFilePaths;
  private readonly INScanSupport _support;

  public ProjectPaths(IEnumerable<AbsoluteFilePath> projectFilePaths, INScanSupport support)
  {
    _projectFilePaths = projectFilePaths;
    _support = support;
  }

  public List<CsharpProjectDto> LoadXmlProjects()
  {
    var projectDtos = _projectFilePaths.Select(LoadXmlProjectFromPath())
      .Where(maybeProject => maybeProject.HasValue)
      .Select(maybeProject => maybeProject.Value()).ToList();
    return projectDtos;
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
    var serializer = CreateXmlSerializer();
    using var fileStream = new FileStream(projectFilePath.ToString(), FileMode.Open);
    XmlProject result = (XmlProject) serializer.Deserialize(fileStream).OrThrow();
    return result;
  }

  private static XmlSerializer CreateXmlSerializer()
  {
    var serializer = new XmlSerializer(typeof(XmlProject), new[]
    {
      typeof(XmlPropertyGroup),
      typeof(XmlItemGroup),
    });
    return serializer;
  }

  private static CsharpProjectDto LoadXmlProject(AbsoluteFilePath projectFilePath)
  {
    var xmlProjectData = DeserializeProjectData(projectFilePath);

    SourceCodeFilePaths.LoadFilesInto(xmlProjectData);
    return xmlProjectData.BuildCsharpProjectDto();
  }

  private static XmlProjectDataAccess DeserializeProjectData(AbsoluteFilePath projectFilePath)
  {
    var deserializeProjectFile = DeserializeProjectFile(projectFilePath);
    var xmlProjectDataAccess = XmlProjectDataAccess.From(
      projectFilePath, 
      deserializeProjectFile);
    return xmlProjectDataAccess;
  }

  private Func<AbsoluteFilePath, Maybe<CsharpProjectDto>> LoadXmlProjectFromPath()
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
        return Maybe<CsharpProjectDto>.Nothing;
      }
    };
  }
}
