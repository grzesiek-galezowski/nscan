using System;
using System.Collections.Generic;
using TddXt.NScan.App;

namespace TddXt.NScan.CompositionRoot
{
  public class DependencyPathFactory : IDependencyPathFactory
  {
    public IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination)
    {
      return new DependencyPathInProgress(
        destination, 
        projects => new ProjectDependencyPath(projects, new ProjectFoundSearchResultFactory()), new List<IReferencedProject>());
    }
  }
}