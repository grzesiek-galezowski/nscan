using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using AtmaFileSystem;
using Buildalyzer;
using Core.Maybe;
using Core.NullableReferenceTypesExtensions;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using NScan.SharedKernel;
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
    SetMsBuildExePath();
    var project = new Project(ProjectRootElement.Open(projectFilePath.ToString()));
    return new CsharpProjectDto(
      new ProjectId(project.FullPath),
      project.Properties.Single(p => p.Name == "AssemblyName").EvaluatedValue,
      project.Properties.Single(p => p.Name == "TargetFramework").EvaluatedValue,
      SourceCodeFilePaths.LoadFiles(project, projectFilePath.ParentDirectory()),
      project.Properties.ToDictionary(p => p.Name, p => p.EvaluatedValue).ToImmutableDictionary(),
      project.Items.Where(item => item.ItemType == "PackageReference")
        .Select(item =>
          new PackageReference(item.EvaluatedInclude, item.Metadata.Single(m => m.Name == "Version").EvaluatedValue))
        .ToImmutableList(),
      ImmutableList<AssemblyReference>.Empty, //bug assembly references
      project.Items.Where(item => item.ItemType == "ProjectReference").Select(item => new ProjectId((projectFilePath.ParentDirectory() + RelativeDirectoryPath(item.EvaluatedInclude)).ToString()))
        .ToImmutableList()
    );

    //bug var xmlProjectData = DeserializeProjectData(projectFilePath);
    //bug 
    //bug SourceCodeFilePaths.LoadFilesInto(xmlProjectData);
    //bug return xmlProjectData.BuildCsharpProjectDto();
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

  private static void SetMsBuildExePath()
  {
    var startInfo = new ProcessStartInfo("dotnet", "--list-sdks") { RedirectStandardOutput = true };

    var process = Process.Start(startInfo).OrThrow();
    process.WaitForExit(1000);

    var output = process.StandardOutput.ReadToEnd();
    var sdkPaths = Regex.Matches(output, "([0-9]+.[0-9]+.[0-9]+) \\[(.*)\\]")
      .OfType<Match>()
      .Select(m => Path.Combine(m.Groups[2].Value, m.Groups[1].Value, "MSBuild.dll"));

    var sdkPath = sdkPaths.Last();
    Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", sdkPath, EnvironmentVariableTarget.Process);
  }
}
