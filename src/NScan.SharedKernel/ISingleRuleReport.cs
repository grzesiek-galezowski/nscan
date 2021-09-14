namespace NScan.SharedKernel
{
  public interface ISingleRuleReport
  {
    bool IsFailed();
    void Add(RuleViolation ruleViolation);
    void AppendTo(IResultBuilder resultBuilder);
  }
}
