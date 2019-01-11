namespace TddXt.NScan.Domain
{
  public interface ISourceCodeFile
  {
    void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report, string ruleDescription);
    void AddNamespaceMappingTo(INamespacesDependenciesCache namespacesDependenciesCache);
  }
}