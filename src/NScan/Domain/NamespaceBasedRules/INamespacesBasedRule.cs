using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.NamespaceBasedRules
{
  public interface INamespacesBasedRule
  {
    string Description();

    void Evaluate(string projectAssemblyName, INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report);
  }
}