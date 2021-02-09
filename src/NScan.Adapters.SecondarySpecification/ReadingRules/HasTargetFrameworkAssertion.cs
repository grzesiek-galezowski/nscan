using System;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapters.SecondarySpecification.ReadingRules
{
  public class HasTargetFrameworkAssertion : RuleUnionDtoAssertion
  {
    private readonly Action<HasTargetFrameworkRuleComplementDto> _action;

    public HasTargetFrameworkAssertion(Action<HasTargetFrameworkRuleComplementDto> action)
    {
      _action = action;
    }

    public override void Visit(HasTargetFrameworkRuleComplementDto dto) => _action(dto);
  }
}
