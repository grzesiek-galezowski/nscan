using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public interface INamespaceBasedRuleTarget
{
  void RefreshNamespacesCache();
  void Evaluate(INamespacesBasedRule rule, IAnalysisReportInProgress report);
}