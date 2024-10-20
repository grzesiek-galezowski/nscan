﻿using System;
using System.Linq;
using AtmaFileSystem;
using Core.Maybe;
using LanguageExt;
using Microsoft.Build.Construction;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

public class MsBuildSolution(Seq<AbsoluteFilePath> projectFilePaths, INScanSupport support)
{
  public static MsBuildSolution From(
    AnyFilePath solutionFilePath, 
    INScanSupport consoleSupport)
  {
    var solutionFile = SolutionFile.Parse(solutionFilePath.ToString());
    var projectFilePaths = solutionFile.ProjectsInOrder.Where(p => p.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat)
      .Select(p => AbsoluteFilePath(p.AbsolutePath));
    var paths = new MsBuildSolution(projectFilePaths.ToSeq(), consoleSupport);
    return paths;
  }

  public Seq<CsharpProjectDto> LoadCsharpProjects()
  {
    var projectDtos = projectFilePaths.Select(LoadCsharpProjectFromPath())
      .Where(maybeProject => maybeProject.HasValue)
      .Select(maybeProject => maybeProject.Value());
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
        support.SkippingProjectBecauseOfError(e, path);
        return Maybe<CsharpProjectDto>.Nothing;
      }
    };
  }
}
