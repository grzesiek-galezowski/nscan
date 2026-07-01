using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AtmaFileSystem;
using Core.Maybe;
using LanguageExt;
using Microsoft.VisualStudio.SolutionPersistence.Serializer;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

public class MsBuildSolution(Seq<AbsoluteFilePath> projectFilePaths, INScanSupport support)
{
  public static async Task<MsBuildSolution> From(
    AnyFilePath solutionFilePath,
    INScanSupport consoleSupport,
    CancellationToken cancellationToken)
  {
    var serializer = SolutionSerializers.GetSerializerByMoniker(solutionFilePath.ToString())
      ?? throw new ArgumentException($"Unsupported solution format: {solutionFilePath}");

    var model = await serializer.OpenAsync(solutionFilePath.ToString(), cancellationToken);
    
    var solutionDir = solutionFilePath.ParentDirectory().Value();
    var projectFilePaths = model.SolutionProjects
      .Select(p => AbsoluteFilePath(Path.GetFullPath(p.FilePath, solutionDir.ToString())));

    return new MsBuildSolution(projectFilePaths.ToSeq(), consoleSupport);
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
