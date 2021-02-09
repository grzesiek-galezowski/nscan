using System;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.Adapters.SecondarySpecification.ReadingRules
{
  public class IndependentRuleComplementDtoAssertion : RuleUnionDtoAssertion
  {
    private readonly Action<IndependentRuleComplementDto> _action;

    public IndependentRuleComplementDtoAssertion(Action<IndependentRuleComplementDto> action)
    {
      _action = action;
    }

    public override void Visit(IndependentRuleComplementDto dto)
    {
      _action(dto);
    }
  }
}
