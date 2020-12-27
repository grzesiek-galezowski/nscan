using System;
using System.Collections.Generic;

namespace NScan.NamespaceBasedRules
{
  public class SourceCodeFileUsingNamespaces : ISourceCodeFileUsingNamespaces
  {
    private readonly IReadOnlyList<string> _usings;
    private readonly IReadOnlyList<string> _declaredNamespaces;

    public SourceCodeFileUsingNamespaces(
      IReadOnlyList<string> usings, //bug value objects
      IReadOnlyList<string> declaredNamespaces //bug value objects
    )
    {
      _usings = usings;
      _declaredNamespaces = declaredNamespaces;
    }

    public void AddNamespaceMappingTo(INamespacesDependenciesCache namespacesDependenciesCache)
    {
      foreach (var declaredNamespace in _declaredNamespaces)
      {
        foreach (var @using in _usings)
        {
          namespacesDependenciesCache.AddMapping(declaredNamespace, @using);
        }
      }

    }
  }
}
