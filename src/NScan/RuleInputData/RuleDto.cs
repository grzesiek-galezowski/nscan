using System;

namespace TddXt.NScan.RuleInputData
{
  public class RuleDto
  {
    public string RuleName { get; set; }
    public Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto> Either { get; set; }
  }
}