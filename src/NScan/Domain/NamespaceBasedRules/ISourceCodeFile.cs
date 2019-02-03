using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.NamespaceBasedRules
{
  public interface ISourceCodeFile
  {
    void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report, string ruleDescription);
    void AddNamespaceMappingTo(INamespacesDependenciesCache namespacesDependenciesCache);
  }
}