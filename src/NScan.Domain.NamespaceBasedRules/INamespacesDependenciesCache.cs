using LanguageExt;
using NScan.Lib;

namespace NScan.NamespaceBasedRules;

public interface INamespacesDependenciesCache
{
  void AddMapping(NamespaceName namespaceName, NamespaceName usingName);
  Seq<NamespaceDependencyPath> RetrieveCycles();
  Seq<NamespaceDependencyPath> RetrievePathsBetween(Pattern fromPattern, Pattern toPattern);
}
