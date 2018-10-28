using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.App;

namespace TddXt.NScan.CompositionRoot
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

    public void BuildStartingFrom(params IDotNetProject[] rootProjects)
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

    public void Add(IProjectDependencyPath projectDependencyPath)
    {
      _projectDependencyPaths.Add(projectDependencyPath);
    }
  }
}