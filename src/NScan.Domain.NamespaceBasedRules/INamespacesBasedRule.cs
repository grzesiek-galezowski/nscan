using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public interface INamespacesBasedRule
{
  public RuleDescription Description();

  void Evaluate(
    AssemblyName projectAssemblyName,
    INamespacesDependenciesCache namespacesCache,
    IAnalysisReportInProgress report);
}