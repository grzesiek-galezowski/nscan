using System.Collections.Generic;
using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public interface INamespacesDependenciesCache
  {
    void AddMapping(NamespaceName namespaceName, NamespaceName usingName);
    List<NamespaceDependencyPath> RetrieveCycles();
    List<NamespaceDependencyPath> RetrievePathsBetween(Pattern fromPattern, Pattern toPattern);
  }
}
