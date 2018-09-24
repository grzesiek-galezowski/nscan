using System.Collections.Generic;
using NScanRoot.App;

namespace NScanRoot.CompositionRoot
{
  public class PathCache : IPathCache, IFinalDependencyPathDestination
  {
    private readonly IDependencyPathFactory _dependencyPathFactory;
    private readonly List<IReadOnlyList<IReferencedProject>> _paths = new List<IReadOnlyList<IReferencedProject>>();

    public PathCache(IDependencyPathFactory dependencyPathFactory)
    {
      _dependencyPathFactory = dependencyPathFactory;
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
      foreach (var path in _paths)
      {
        rule.Check(path, report);
      }
    }

    public void Add(IReadOnlyList<IReferencedProject> finalPath)
    {
      _paths.Add(finalPath);
    }
  }
}