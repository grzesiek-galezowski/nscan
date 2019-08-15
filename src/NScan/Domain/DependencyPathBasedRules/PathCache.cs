using System.Collections.Generic;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public class PathCache : IPathCache, IFinalDependencyPathDestination
  {
    private readonly IDependencyPathFactory _dependencyPathFactory;
    private readonly List<IProjectDependencyPath> _projectDependencyPaths;

    public PathCache(IDependencyPathFactory dependencyPathFactory)
    {
      _dependencyPathFactory = dependencyPathFactory;
      _projectDependencyPaths = new List<IProjectDependencyPath>();
    }

    public void Add(IProjectDependencyPath projectDependencyPath)
    {
      _projectDependencyPaths.Add(projectDependencyPath);
    }

    public void BuildStartingFrom(params IDependencyPathBasedRuleTarget[] rootProjects)
    {
      foreach (var dotNetProject in rootProjects)
      {
        dotNetProject.FillAllBranchesOf(_dependencyPathFactory.NewDependencyPathFor(this));
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
}