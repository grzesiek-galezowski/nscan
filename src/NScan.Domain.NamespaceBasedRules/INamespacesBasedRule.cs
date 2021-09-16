using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules
{
  public interface INamespacesBasedRule
  {
    public RuleDescription Description();

    void Evaluate(string projectAssemblyName, INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report);
  }
}
