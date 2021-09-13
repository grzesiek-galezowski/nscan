namespace NScan.SharedKernel
{
  public interface IAnalysisReportInProgress
  {
    void StartedCheckingTarget(string assemblyName);
    void FinishedEvaluatingRule(string ruleDescription);
    void AsString(IResultBuilder resultBuilder); //bug make void
    bool IsSuccessful();
    void Add(RuleViolation ruleViolation);
  }

  public interface IResultBuilder //bug
  {
    void AppendViolations(RuleDescription ruleDescription, string violationsString);

    void AppendOk(RuleDescription ruleDescription);
    void AppendRuleSeparator();
    string Text();
  }

  public interface IRuleReport //bug
  {
    string ViolationsString();
    bool IsSuccessful();
    void AddViolation(RuleViolation ruleViolation);
    void AppendTo(IResultBuilder resultBuilder, RuleDescription ruleDescription);
  }


}
