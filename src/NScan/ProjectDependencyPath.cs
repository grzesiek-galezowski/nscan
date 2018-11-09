using System;
using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using static TddXt.NScan.ProjectDependencyPath.Predicates;

namespace TddXt.NScan
{
  public interface IProjectDependencyPath
  {
    IProjectSearchResult AssemblyMatching(IDescribedDependencyCondition condition,
      IProjectSearchResult depending);

    IProjectSearchResult AssemblyWithNameMatching(Pattern pattern);

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


    public IProjectSearchResult AssemblyMatching(IDescribedDependencyCondition condition,
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


    public IProjectSearchResult AssemblyWithNameMatching(Pattern pattern)
    {
      if (_path.Any(AssemblyNameMatches(pattern)))
      {
        var (foundProject, occurenceIndex)
          = FindWithIndexWhere(AssemblyNameMatches(pattern));
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
      internal static Func<IReferencedProject, bool> ProjectMeets(IDescribedDependencyCondition nextAssemblyMatchesCondition,
        IProjectSearchResult depending)
      {
        return project => nextAssemblyMatchesCondition.Matches(depending, project);
      }

      internal static Func<IReferencedProject, bool> AssemblyNameMatches(Pattern pattern)
      {
        return p => p.HasProjectAssemblyNameMatching(pattern);
      }
    }



  }
}