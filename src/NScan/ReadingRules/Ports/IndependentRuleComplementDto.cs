using GlobExpressions;

namespace TddXt.NScan.ReadingRules.Ports
{
  public class IndependentRuleComplementDto
  {
    public Glob DependencyPattern { get; set; } //bug Pattern instead of Glob?
    public string DependencyType { get; set; }
    public string RuleName { get; } = RuleNames.IndependentOf;
    public Pattern DependingPattern { get; set; }
  }
}