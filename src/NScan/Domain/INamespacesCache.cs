using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface INamespacesCache
  {
    void BuildForEachOf(IReadOnlyCollection<IDotNetProject> values);
  }
}