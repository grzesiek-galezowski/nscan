using System;
using System.Collections.Generic;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class PathCache : IPathCache, IFinalDependencyPathDestination
  {
    private readonly IDependencyPathFactory _dependencyPathFactory;

    public PathCache(IDependencyPathFactory dependencyPathFactory)
    {
      _dependencyPathFactory = dependencyPathFactory;
    }

    public void BuildStartingFrom(params IDotNetProject[] rootProjects)
    {
      foreach (var dotNetProject in rootProjects)
      {
        dotNetProject.FillAllBranchesOf(_dependencyPathFactory.CreateNewDependencyPathFor(this));
      }
    }

    public void Add(IReadOnlyList<IReferencedProject> finalPath)
    {
      throw new NotImplementedException();
    }
  }
}