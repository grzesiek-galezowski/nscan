using System.Collections.Generic;
using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public interface INamespacesDependenciesCache
  {
    void AddMapping(NamespaceName namespaceName, NamespaceName usingName);
    IReadOnlyList<IReadOnlyList<NamespaceName>> RetrieveCycles();
    IReadOnlyList<IReadOnlyList<NamespaceName>> RetrievePathsBetween(Pattern fromPattern, Pattern toPattern);
  }
}
