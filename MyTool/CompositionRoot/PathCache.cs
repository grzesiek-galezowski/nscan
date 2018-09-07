using System;
using System.Collections.Generic;
using MyTool.App;

namespace MyTool.CompositionRoot
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

    public void Check(IDependencyRule ruleSet, IAnalysisReportInProgress report)
    {
      /*foreach (var path in _paths)
      {
        //todo hmm... probably should be the other way round - each rule should be checked on all paths...
        //todo take a step back and redesign
        ruleSet.Check(path, report);
      }*/
    }

    public void Add(IReadOnlyList<IReferencedProject> finalPath)
    {
      _paths.Add(finalPath);
    }
  }
}