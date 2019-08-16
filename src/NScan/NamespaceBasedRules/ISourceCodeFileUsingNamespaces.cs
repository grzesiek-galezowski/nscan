namespace NScan.Domain.NamespaceBasedRules
{
  public interface ISourceCodeFileUsingNamespaces
  {
    void AddNamespaceMappingTo(INamespacesDependenciesCache namespacesDependenciesCache);
  }
}