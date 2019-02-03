using System.Collections.Generic;

namespace TddXt.NScan.Domain.NamespaceBasedRules
{
  public interface INamespacesDependenciesCache
  {
    void AddMapping(string namespaceName, string usingName);
    IReadOnlyList<IReadOnlyList<string>> RetrieveCycles();
  }
}