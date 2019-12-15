using System.Collections.Generic;
using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public interface INamespacesDependenciesCache
  {
    void AddMapping(string namespaceName, string usingName);
    IReadOnlyList<IReadOnlyList<string>> RetrieveCycles();
    IReadOnlyList<IReadOnlyList<string>> RetrievePathsBetween(Pattern fromPattern, Pattern toPattern);
  }
}