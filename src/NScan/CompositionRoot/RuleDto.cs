using GlobExpressions;

namespace TddXt.NScan.CompositionRoot
{
  public class RuleDto
  {
    public Pattern DependingPattern { get; set; }
    public IndependentRuleComplementDto IndependentRuleComplement { get; set; }
    public CorrectNamespacesRuleComplementDto CorrectNamespacesRuleComplement { get; set; }
    public string RuleName { get; set; }
  }

  public class ComplementDto
  {
    public IndependentRuleComplementDto IndependentRuleComplement { get; set; }
    public CorrectNamespacesRuleComplementDto CorrectNamespacesRuleComplement { get; set; }
    public string RuleName { get; set; }
  }

  public class IndependentRuleComplementDto
  {
    public Glob DependencyPattern { get; set; }
    public string DependencyType { get; set; }
  }

  public class CorrectNamespacesRuleComplementDto
  {
  }
}