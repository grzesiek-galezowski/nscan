using System;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

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