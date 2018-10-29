using System;
using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.App;
using static TddXt.NScan.ProjectDependencyPath.Predicates;

namespace TddXt.NScan
{
  public interface IProjectDependencyPath
  {
    IProjectSearchResult AssemblyMatching(IDependencyCondition condition,
      IProjectSearchResult depending);

    IProjectSearchResult AssemblyWithNameMatching(Glob.Glob glob);

    IReadOnlyList<IReferencedProject> SegmentBetween(IProjectSearchResult dependingProjectSearchResult,
      IProjectSearchResult dependency);
  }

  public class ProjectDependencyPath : IProjectDependencyPath
  {
    private readonly IReadOnlyList<IReferencedProject> _path;
    private readonly IProjectFoundSearchResultFactory _projectFoundSearchResultFactory;

    public ProjectDependencyPath(
      IReadOnlyList<IReferencedProject> path,
      IProjectFoundSearchResultFactory projectFoundSearchResultFactory)
    {
      _path = path;
      _projectFoundSearchResultFactory = projectFoundSearchResultFactory;
    }


    public IProjectSearchResult AssemblyMatching(IDependencyCondition condition,
      IProjectSearchResult depending)
    {
      if (_path.Any(ProjectMeets(condition, depending)))
      {
        var (projectFound, indexFound) = FindWithIndexWhere(ProjectMeets(condition, depending));
        return _projectFoundSearchResultFactory.ItemFound(projectFound, indexFound);
      }
      else
      {
        return _projectFoundSearchResultFactory.ItemNotFound();
      }
    }


    public IProjectSearchResult AssemblyWithNameMatching(Glob.Glob glob)
    {
      if (_path.Any(AssemblyNameMatches(glob)))
      {
        var (foundProject, occurenceIndex)
          = FindWithIndexWhere(AssemblyNameMatches(glob));
        return _projectFoundSearchResultFactory.ItemFound(foundProject, occurenceIndex);
      }
      else

      {
        return _projectFoundSearchResultFactory.ItemNotFound();
      }
    }

    public IReadOnlyList<IReferencedProject> SegmentBetween(IProjectSearchResult dependingProjectSearchResult,
      IProjectSearchResult dependency)
    {
      return dependingProjectSearchResult.SegmentEndingWith(dependency, _path);
    }

    private (IReferencedProject, int) FindWithIndexWhere(Func<IReferencedProject, bool> assemblyNameMatches)
    {
      return _path
        .Select((project, i) => (project, i))
        .First(tuple => assemblyNameMatches(tuple.Item1));
    }


    internal static class Predicates
    {
      internal static Func<IReferencedProject, bool> ProjectMeets(IDependencyCondition nextAssemblyMatchesCondition,
        IProjectSearchResult depending)
      {
        return project => nextAssemblyMatchesCondition.Matches(project, depending);
      }

      internal static Func<IReferencedProject, bool> AssemblyNameMatches(Glob.Glob glob)
      {
        return p => p.HasAssemblyNameMatching(glob);
      }
    }



  }
}