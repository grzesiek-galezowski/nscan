using System;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
{
  public class NoUsingsRuleComplementDtoAssertion : RuleUnionDtoAssertion
  {
    private readonly Action<NoUsingsRuleComplementDto> _action;

    public NoUsingsRuleComplementDtoAssertion(Action<NoUsingsRuleComplementDto> action)
    {
      _action = action;
    }

    public override void Visit(NoUsingsRuleComplementDto dto)
    {
      _action(dto);
    }
  }
}