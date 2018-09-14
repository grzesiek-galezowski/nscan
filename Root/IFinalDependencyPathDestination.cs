using System.Collections.Generic;
using MyTool.App;

namespace MyTool
{
  public interface IFinalDependencyPathDestination
  {
    void Add(IReadOnlyList<IReferencedProject> finalPath);
  }
}