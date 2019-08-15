using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.NamespaceBasedRules
{
  public interface INamespacesBasedRule
  {
    string Description();

    void Evaluate(string projectAssemblyName, INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report);
  }
}