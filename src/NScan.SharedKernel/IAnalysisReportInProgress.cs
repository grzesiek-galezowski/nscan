namespace NScan.SharedKernel
{
  public interface IAnalysisReportInProgress
  {
    void StartedCheckingTarget(string assemblyName);
    void FinishedEvaluatingRule(string ruleDescription);
    string AsString();
    bool HasViolations();
    void Add(RuleViolation ruleViolation);
  }
}
