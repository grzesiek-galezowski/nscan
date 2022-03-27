namespace NScan.NamespaceBasedRules;

public interface ISourceCodeFileUsingNamespaces
{
  void AddNamespaceMappingTo(INamespacesDependenciesCache namespacesDependenciesCache);
}