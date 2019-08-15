using System.Collections.Generic;

namespace NScan.Domain.Domain.NamespaceBasedRules
{
  public interface INamespacesDependenciesCache
  {
    void AddMapping(string namespaceName, string usingName);
    IReadOnlyList<IReadOnlyList<string>> RetrieveCycles();
  }
}