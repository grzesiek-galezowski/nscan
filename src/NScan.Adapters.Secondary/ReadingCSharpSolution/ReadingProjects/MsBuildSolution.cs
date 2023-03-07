using System;
using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using Buildalyzer;
using Core.Maybe;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

public class MsBuildSolution
{
  private readonly IEnumerable<AbsoluteFilePath> _projectFilePaths;
  private readonly INScanSupport _support;

  public static MsBuildSolution From(
    AnyFilePath solutionFilePath, 
    INScanSupport consoleSupport)
  {
    var analyzerManager = new AnalyzerManager(solutionFilePath.ToString());
    var projectFilePaths = analyzerManager.Projects
      .Select(p => p.Value.ProjectFile.Path)
      .Select(AbsoluteFilePath).ToList();
    var paths = new MsBuildSolution(projectFilePaths, consoleSupport);
    return paths;
  }

  public MsBuildSolution(IEnumerable<AbsoluteFilePath> projectFilePaths, INScanSupport support)
  {
    _projectFilePaths = projectFilePaths;
    _support = support;
  }

  public List<CsharpProjectDto> LoadCsharpProjects()
  {
    var projectDtos = _projectFilePaths.Select(LoadCsharpProjectFromPath())
      .Where(maybeProject => maybeProject.HasValue)
      .Select(maybeProject => maybeProject.Value()).ToList();
    return projectDtos;
  }

  private static CsharpProjectDto LoadProjectDto(AbsoluteFilePath projectFilePath)
  {
    var msBuildProject = MsBuildProject.From(projectFilePath);
    return new CsharpProjectDto(
      msBuildProject.Id(),
      msBuildProject.AssemblyName(), 
      msBuildProject.LoadSourceCodeFiles(),
      msBuildProject.Properties(), 
      msBuildProject.PackageReferences(), 
      msBuildProject.AssemblyReferences(), 
      msBuildProject.ProjectReferences(),
      msBuildProject.TargetFrameworks()
    );
  }

  private Func<AbsoluteFilePath, Maybe<CsharpProjectDto>> LoadCsharpProjectFromPath()
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
