using System;
using NScan.SharedKernel.RuleDtos;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
{
  public class NoCircularUsingsRuleComplementDtoAssertion : RuleUnionDtoAssertion
  {
    private readonly Action<NoCircularUsingsRuleComplementDto> _action;

    public NoCircularUsingsRuleComplementDtoAssertion(Action<NoCircularUsingsRuleComplementDto> action)
    {
      _action = action;
    }

    public override void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      _action(dto);
    }
  }
}