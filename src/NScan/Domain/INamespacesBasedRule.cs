namespace TddXt.NScan.Domain
{
  public interface INamespacesBasedRule
  {
    string Description();
    void Evaluate(INamespacesDependenciesCache namespacesCache, IAnalysisReportInProgress report);
  }
}