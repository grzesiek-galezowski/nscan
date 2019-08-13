using System;
using NScan.SharedKernel.Ports;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
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