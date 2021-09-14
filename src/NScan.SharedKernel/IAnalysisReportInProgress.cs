namespace NScan.SharedKernel
{
  public interface IAnalysisReportInProgress
  {
    void StartedCheckingTarget(string assemblyName);
    void FinishedEvaluatingRule(string ruleDescription);
    void AsString(IResultBuilder resultBuilder);
    bool IsFailure();
    void Add(RuleViolation ruleViolation);
  }
}
