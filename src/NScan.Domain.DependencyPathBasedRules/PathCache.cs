namespace NScan.DependencyPathBasedRules;

public class PathCache(IDependencyPathFactory dependencyPathFactory) : IPathCache, IFinalDependencyPathDestination
{
  private Seq<IProjectDependencyPath> _projectDependencyPaths;

  public void Add(IProjectDependencyPath projectDependencyPath)
  {
    _projectDependencyPaths = _projectDependencyPaths.Add(projectDependencyPath);
  }

  public void BuildStartingFrom(Seq<IDependencyPathBasedRuleTarget> rootProjects)
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
