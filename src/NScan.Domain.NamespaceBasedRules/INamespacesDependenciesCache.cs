using System.Collections.Generic;
using LanguageExt;
using NScan.Lib;

namespace NScan.NamespaceBasedRules;

public interface INamespacesDependenciesCache
{
  void AddMapping(NamespaceName namespaceName, NamespaceName usingName);
  Arr<NamespaceDependencyPath> RetrieveCycles();
  Arr<NamespaceDependencyPath> RetrievePathsBetween(Pattern fromPattern, Pattern toPattern);
}
