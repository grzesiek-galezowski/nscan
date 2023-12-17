using System;
using System.Collections.Generic;
using System.Linq;
using NScan.Lib;

namespace NScan.DependencyPathBasedRules;

public interface IProjectDependencyPath
{
  IProjectSearchResult AssemblyMatching(IDescribedDependencyCondition condition,
    IProjectSearchResult depending);

  IProjectSearchResult AssemblyWithNameMatching(Pattern pattern);

  IReadOnlyList<IDependencyPathBasedRuleTarget> SegmentBetween(
    IProjectSearchResult dependingProjectSearchResult,
    IProjectSearchResult dependency);
}

public class ProjectDependencyPath(
  IReadOnlyList<IDependencyPathBasedRuleTarget> path,
  IProjectFoundSearchResultFactory projectFoundSearchResultFactory)
  : IProjectDependencyPath
{
  public IProjectSearchResult AssemblyMatching(IDescribedDependencyCondition condition,
    IProjectSearchResult depending)
  {
    if (path.Any(Predicates.ProjectMeets(condition, depending)))
    {
      var (projectFound, indexFound) = FindWithIndexWhere(Predicates.ProjectMeets(condition, depending));
      return projectFoundSearchResultFactory.ItemFound(projectFound, indexFound);
    }
    else
    {
      return projectFoundSearchResultFactory.ItemNotFound();
    }
  }


  public IProjectSearchResult AssemblyWithNameMatching(Pattern pattern)
  {
    if (path.Any(Predicates.AssemblyNameMatches(pattern)))
    {
      var (foundProject, occurenceIndex)
        = FindWithIndexWhere(Predicates.AssemblyNameMatches(pattern));
      return projectFoundSearchResultFactory.ItemFound(foundProject, occurenceIndex);
    }
    else

    {
      return projectFoundSearchResultFactory.ItemNotFound();
    }
  }

  public IReadOnlyList<IDependencyPathBasedRuleTarget> SegmentBetween(IProjectSearchResult dependingProjectSearchResult,
    IProjectSearchResult dependency)
  {
    return dependingProjectSearchResult.SegmentEndingWith(dependency, path);
  }

  private (IDependencyPathBasedRuleTarget, int) FindWithIndexWhere(Func<IDependencyPathBasedRuleTarget, bool> assemblyNameMatches)
  {
    return path
      .Select((project, i) => (project, i))
      .First(tuple => assemblyNameMatches(tuple.Item1));
  }


  private static class Predicates
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
