using System.Collections.Generic;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class DependencyPathFactory : IDependencyPathFactory
  {
    public IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination)
    {
      return new DependencyPathInProgress(destination, new List<IReferencedProject>());
    }
  }
}