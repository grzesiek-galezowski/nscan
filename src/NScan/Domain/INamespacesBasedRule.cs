namespace TddXt.NScan.Domain
{
  public interface INamespacesBasedRule
  {
    string Description();
    void Evaluate(string projectAssemblyName, INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report);
  }
}