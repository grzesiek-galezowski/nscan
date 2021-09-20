namespace NScan.SharedKernel
{
  public interface IAnalysisReportInProgress
  {
    void StartedCheckingTarget(AssemblyName assemblyName);
    void AsString(IResultBuilder resultBuilder);
    bool IsFailure();
    void Add(RuleViolation ruleViolation);
    void FinishedEvaluatingRule(RuleDescription ruleDescription);
  }
}
