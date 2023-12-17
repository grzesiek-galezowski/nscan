using System.Collections.Generic;

namespace NScan.NamespaceBasedRules;

public class SourceCodeFileUsingNamespaces(
  IReadOnlyList<NamespaceName> usings,
  IReadOnlyList<NamespaceName> declaredNamespaces)
  : ISourceCodeFileUsingNamespaces
{
  public void AddNamespaceMappingTo(INamespacesDependenciesCache namespacesDependenciesCache)
  {
    foreach (var declaredNamespace in declaredNamespaces)
    {
      foreach (var @using in usings)
      {
        namespacesDependenciesCache
          .AddMapping(declaredNamespace, @using);
      }
    }

  }
}
