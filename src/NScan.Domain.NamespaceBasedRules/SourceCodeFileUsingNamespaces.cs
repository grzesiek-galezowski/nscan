using System.Collections.Generic;

namespace NScan.NamespaceBasedRules
{
  public class SourceCodeFileUsingNamespaces : ISourceCodeFileUsingNamespaces
  {
    private readonly IReadOnlyList<NamespaceName> _usings;
    private readonly IReadOnlyList<NamespaceName> _declaredNamespaces;

    public SourceCodeFileUsingNamespaces(
      IReadOnlyList<NamespaceName> usings,
      IReadOnlyList<NamespaceName> declaredNamespaces
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
          namespacesDependenciesCache
            .AddMapping(declaredNamespace, @using);
        }
      }

    }
  }
}
