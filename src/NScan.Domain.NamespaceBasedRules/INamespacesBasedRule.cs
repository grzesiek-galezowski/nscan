using NScan.SharedKernel;

namespace NScan.Domain.NamespaceBasedRules
{
  public interface INamespacesBasedRule
  {
    string Description();

    void Evaluate(string projectAssemblyName, INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report);
  }
}