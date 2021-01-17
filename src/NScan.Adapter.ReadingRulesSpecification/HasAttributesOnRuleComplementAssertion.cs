using System;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapter.ReadingRulesSpecification
{
  public class HasAttributesOnRuleComplementAssertion : RuleUnionDtoAssertion
  {
    private readonly Action<HasAttributesOnRuleComplementDto> _action;

    public HasAttributesOnRuleComplementAssertion(Action<HasAttributesOnRuleComplementDto> action)
    {
      _action = action;
    }

    public override void Visit(HasAttributesOnRuleComplementDto dto)
    {
      _action(dto);
    }
  }
}
