using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface INamespacesDependenciesCache
  {
    void Build();
    void AddMapping(string namespaceName, string usingName);
    IReadOnlyList<IReadOnlyList<string>> RetrieveCycles();
  }
}