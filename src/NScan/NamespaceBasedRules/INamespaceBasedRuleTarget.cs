using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.NamespaceBasedRules
{
  public interface INamespaceBasedRuleTarget
  {
    void RefreshNamespacesCache();
    void Evaluate(INamespacesBasedRule rule, IAnalysisReportInProgress report);
  }
}