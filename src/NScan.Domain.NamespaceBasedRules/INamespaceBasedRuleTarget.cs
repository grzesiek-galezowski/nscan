using NScan.SharedKernel;

namespace NScan.Domain.NamespaceBasedRules
{
  public interface INamespaceBasedRuleTarget
  {
    void RefreshNamespacesCache();
    void Evaluate(INamespacesBasedRule rule, IAnalysisReportInProgress report);
  }
}