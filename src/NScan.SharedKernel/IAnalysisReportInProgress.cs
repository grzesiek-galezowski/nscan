namespace NScan.SharedKernel
{
  public interface IAnalysisReportInProgress
  {
    void StartedCheckingTarget(string assemblyName);
    void AsString(IResultBuilder resultBuilder);
    bool IsFailure();
    void Add(RuleViolation ruleViolation);
    void FinishedEvaluatingRule(RuleDescription ruleDescription);
  }
}
