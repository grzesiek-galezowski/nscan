namespace NScan.Domain.Domain.NamespaceBasedRules
{
  public interface ISourceCodeFileUsingNamespaces
  {
    void AddNamespaceMappingTo(INamespacesDependenciesCache namespacesDependenciesCache);
  }
}