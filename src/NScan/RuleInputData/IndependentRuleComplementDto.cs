using GlobExpressions;
using TddXt.NScan.Domain;

namespace TddXt.NScan.RuleInputData
{
  public class IndependentRuleComplementDto
  {
    public Glob DependencyPattern { get; set; }
    public string DependencyType { get; set; }
    public string RuleName { get; set; }
    public Pattern DependingPattern { get; set; }
  }
}