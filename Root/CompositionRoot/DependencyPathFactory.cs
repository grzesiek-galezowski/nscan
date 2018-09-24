using System.Collections.Generic;
using NScanRoot.App;

namespace NScanRoot.CompositionRoot
{
  public class DependencyPathFactory : IDependencyPathFactory
  {
    public IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination)
    {
      return new DependencyPathInProgress(destination, new List<IReferencedProject>());
    }
  }
}