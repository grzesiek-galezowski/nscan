namespace TddXt.NScan.Domain
{
  public interface IProjectScopedRuleTarget
  {
    void Evaluate(IProjectScopedRule rule, IAnalysisReportInProgress report);
  }
}