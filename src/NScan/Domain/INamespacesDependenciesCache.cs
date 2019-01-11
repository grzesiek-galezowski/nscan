namespace TddXt.NScan.Domain
{
  public interface INamespacesDependenciesCache
  {
    void Build();
    void AddMapping(string namespaceName, string usingName);
  }
}