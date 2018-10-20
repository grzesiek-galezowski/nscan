using System.Collections.Generic;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IFinalDependencyPathDestination
  {
    void Add(IReadOnlyList<IReferencedProject> finalPath);
  }
}