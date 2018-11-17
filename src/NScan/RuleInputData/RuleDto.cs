using System;
using TddXt.NScan.Domain;

namespace TddXt.NScan.RuleInputData
{
  public class RuleDto
  {
    public Pattern DependingPattern { get; set; }
    public IndependentRuleComplementDto IndependentRuleComplement { get; set; }
    public CorrectNamespacesRuleComplementDto CorrectNamespacesRuleComplement { get; set; }
    public string RuleName { get; set; }
  }
}