using System.Collections.Generic;

namespace NScan.DependencyPathBasedRules;

public class PathCache(IDependencyPathFactory dependencyPathFactory) : IPathCache, IFinalDependencyPathDestination
{
  private readonly List<IProjectDependencyPath> _projectDependencyPaths = new();

  public void Add(IProjectDependencyPath projectDependencyPath)
  {
    _projectDependencyPaths.Add(projectDependencyPath);
  }

  public void BuildStartingFrom(IEnumerable<IDependencyPathBasedRuleTarget> rootProjects)
  {
    foreach (var dotNetProject in rootProjects)
    {
      dotNetProject.FillAllBranchesOf(dependencyPathFactory.NewDependencyPathFor(this));
    }
  }

  public void Check(IDependencyRule rule, IAnalysisReportInProgress report)
  {
    foreach (var path in _projectDependencyPaths)
    {
      rule.Check(report, path);
    }
  }
}
