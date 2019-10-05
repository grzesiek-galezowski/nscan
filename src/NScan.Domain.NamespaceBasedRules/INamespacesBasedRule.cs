using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules
{
  public interface INamespacesBasedRule
  {
    string Description();

    void Evaluate(string projectAssemblyName, INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report);
  }
}