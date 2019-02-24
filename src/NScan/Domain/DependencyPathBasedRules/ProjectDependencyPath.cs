using System;
using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IProjectDependencyPath
  {
    IProjectSearchResult AssemblyMatching(IDescribedDependencyCondition condition,
      IProjectSearchResult depending);

    IProjectSearchResult AssemblyWithNameMatching(Pattern pattern);

    IReadOnlyList<IDependencyPathBasedRuleTarget> SegmentBetween(IProjectSearchResult dependingProjectSearchResult,
      IProjectSearchResult dependency);
  }

  public class ProjectDependencyPath : IProjectDependencyPath
  {
    private readonly IReadOnlyList<IDependencyPathBasedRuleTarget> _path;
    private readonly IProjectFoundSearchResultFactory _projectFoundSearchResultFactory;

    public ProjectDependencyPath(
      IReadOnlyList<IDependencyPathBasedRuleTarget> path,
      IProjectFoundSearchResultFactory projectFoundSearchResultFactory)
    {
      _path = path;
      _projectFoundSearchResultFactory = projectFoundSearchResultFactory;
    }


    public IProjectSearchResult AssemblyMatching(IDescribedDependencyCondition condition,
      IProjectSearchResult depending)
    {
      if (_path.Any(Predicates.ProjectMeets(condition, depending)))
      {
        var (projectFound, indexFound) = FindWithIndexWhere(Predicates.ProjectMeets(condition, depending));
        return _projectFoundSearchResultFactory.ItemFound(projectFound, indexFound);
      }
      else
      {
        return _projectFoundSearchResultFactory.ItemNotFound();
      }
    }


    public IProjectSearchResult AssemblyWithNameMatching(Pattern pattern)
    {
      if (_path.Any(Predicates.AssemblyNameMatches(pattern)))
      {
        var (foundProject, occurenceIndex)
          = FindWithIndexWhere(Predicates.AssemblyNameMatches(pattern));
        return _projectFoundSearchResultFactory.ItemFound(foundProject, occurenceIndex);
      }
      else

      {
        return _projectFoundSearchResultFactory.ItemNotFound();
      }
    }

    public IReadOnlyList<IDependencyPathBasedRuleTarget> SegmentBetween(IProjectSearchResult dependingProjectSearchResult,
      IProjectSearchResult dependency)
    {
      return dependingProjectSearchResult.SegmentEndingWith(dependency, _path);
    }

    private (IDependencyPathBasedRuleTarget, int) FindWithIndexWhere(Func<IDependencyPathBasedRuleTarget, bool> assemblyNameMatches)
    {
      return _path
        .Select((project, i) => (project, i))
        .First(tuple => assemblyNameMatches(tuple.Item1));
    }


    internal static class Predicates
    {
      internal static Func<IDependencyPathBasedRuleTarget, bool> ProjectMeets(IDescribedDependencyCondition nextAssemblyMatchesCondition,
        IProjectSearchResult depending)
      {
        return project => nextAssemblyMatchesCondition.Matches(depending, project);
      }

      internal static Func<IDependencyPathBasedRuleTarget, bool> AssemblyNameMatches(Pattern pattern)
      {
        return p => p.HasProjectAssemblyNameMatching(pattern);
      }
    }
  }
}