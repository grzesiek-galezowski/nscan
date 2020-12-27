using System.Collections.Generic;
using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public interface INamespacesDependenciesCache
  {
    void AddMapping(string namespaceName, string usingName);
    IReadOnlyList<IReadOnlyList<NamespaceName>> RetrieveCycles();
    IReadOnlyList<IReadOnlyList<NamespaceName>> RetrievePathsBetween(Pattern fromPattern, Pattern toPattern);
  }
}