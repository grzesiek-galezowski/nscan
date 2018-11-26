namespace TddXt.NScan.Domain
{
  public interface IProjectScopedRule
  {
    void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report);
  }
}