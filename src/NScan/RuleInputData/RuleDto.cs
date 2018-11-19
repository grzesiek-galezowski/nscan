using System;
using TddXt.NScan.Domain;

namespace TddXt.NScan.RuleInputData
{
  public class RuleDto
  {
    public Pattern DependingPattern { get; set; }

    public IndependentRuleComplementDto IndependentRuleComplement
    {
      get => Either.Left;
      set
      {
        Either = new Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto>()
        {
          Left = value
        };
      }
    }

    public CorrectNamespacesRuleComplementDto CorrectNamespacesRuleComplement { get; set; }
    public string RuleName { get; set; }
    public Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto> Either { get; set; }
  }
}