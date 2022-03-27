using System;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public class HasPropertyAssertion : RuleUnionDtoAssertion
{
  private readonly Action<HasPropertyRuleComplementDto> _action;

  public HasPropertyAssertion(Action<HasPropertyRuleComplementDto> action)
  {
    _action = action;
  }

  public override void Visit(HasPropertyRuleComplementDto dto) => _action(dto);
}