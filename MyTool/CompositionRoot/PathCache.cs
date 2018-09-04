using System;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class PathCache : IPathCache, IDependencyPathDestination
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
        dotNetProject.Accept(_dependencyPathFactory.CreateNewDependencyPathFor(this));
      }
    }
  }
}