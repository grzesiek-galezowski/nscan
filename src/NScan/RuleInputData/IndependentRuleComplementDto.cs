using GlobExpressions;

namespace TddXt.NScan.RuleInputData
{
  public class IndependentRuleComplementDto
  {
    public Glob DependencyPattern { get; set; }
    public string DependencyType { get; set; }
  }
}