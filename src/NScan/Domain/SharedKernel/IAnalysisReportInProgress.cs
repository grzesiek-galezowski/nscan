namespace TddXt.NScan.Domain.SharedKernel
{
  public interface IAnalysisReportInProgress
  {
    void FinishedChecking(string ruleDescription);
    string AsString();
    bool HasViolations();
    void Add(RuleViolation ruleViolation);
  }
}