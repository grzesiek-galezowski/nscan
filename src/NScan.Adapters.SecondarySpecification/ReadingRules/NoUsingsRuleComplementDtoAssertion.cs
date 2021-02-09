using System;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.Adapters.SecondarySpecification.ReadingRules
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
