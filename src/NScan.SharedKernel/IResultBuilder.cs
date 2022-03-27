namespace NScan.SharedKernel;

public interface IResultBuilder
{
  void AppendViolations(RuleDescription ruleDescription, string violationsString);

  void AppendOk(RuleDescription ruleDescription);
  void AppendRuleSeparator();
  string Text();
}