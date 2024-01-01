using LanguageExt;

namespace NScan.NamespaceBasedRules;

public class SourceCodeFileUsingNamespaces(
  Seq<NamespaceName> usings,
  Seq<NamespaceName> declaredNamespaces)
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
