using System.Collections.Generic;
using NScanRoot.App;

namespace NScanRoot
{
  public interface IFinalDependencyPathDestination
  {
    void Add(IReadOnlyList<IReferencedProject> finalPath);
  }
}