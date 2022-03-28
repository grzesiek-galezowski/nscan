using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AtmaFileSystem;
using Buildalyzer;
using Core.Maybe;
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

  private static CsharpProjectDto LoadProjectDto(AbsoluteFilePath projectFilePath)
  {
    //bug refactor this
    MsBuild.ExePathAsEnvironmentVariable();
    var project = new Project(ProjectRootElement.Open(projectFilePath.ToString()));
    //bug throw new NotImplementedException("missing assembly references!");
    return new CsharpProjectDto(
      new ProjectId(project.FullPath),
      project.Properties.Single(p => p.Name == "AssemblyName").EvaluatedValue,
      project.Properties.Single(p => p.Name == "TargetFramework").EvaluatedValue,
      SourceCodeFilePaths.LoadFiles(project, projectFilePath.ParentDirectory()),
      project.Properties.ToDictionary(p => p.Name, p => p.EvaluatedValue).ToImmutableDictionary(),
      project.Items
        .Where(item => item.ItemType == "PackageReference")
        .Where(item => item.Metadata.Single(m => m.Name == "IsImplicitlyDefined").EvaluatedValue == "false") //to filter out .net sdk dependency
        .Select(item =>
          new PackageReference(item.EvaluatedInclude, item.Metadata.Single(m => m.Name == "Version").EvaluatedValue))
        .ToImmutableList(),
      project.Items.Where(item => item.ItemType == "AssemblyReference")
        .Select(item => new AssemblyReference(item.EvaluatedInclude, item.GetMetadata("HintPath").EvaluatedValue))
        .ToImmutableList(),
      project.Items.Where(item => item.ItemType == "ProjectReference")
        .Select(item => new ProjectId((projectFilePath.ParentDirectory() + RelativeDirectoryPath(item.EvaluatedInclude)).ToString()))
        .ToImmutableList()
    );
  }

  private Func<AbsoluteFilePath, Maybe<CsharpProjectDto>> LoadXmlProjectFromPath()
  {
    return path =>
    {
      try
      {
        return LoadProjectDto(path).Just();
      }
      catch (InvalidOperationException e)
      {
        _support.SkippingProjectBecauseOfError(e, path);
        return Maybe<CsharpProjectDto>.Nothing;
      }
    };
  }
}
