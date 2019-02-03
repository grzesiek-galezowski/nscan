namespace TddXt.NScan.Domain
{
  public interface INamespaceBasedRuleTarget
  {
    void RefreshNamespacesCache();
    void Evaluate(INamespacesBasedRule rule, IAnalysisReportInProgress report);
  }
}