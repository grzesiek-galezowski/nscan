using System;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
{
  public class CorrectNamespacesRuleComplementDtoAssertion : RuleUnionDtoAssertion
  {
    private readonly Action<CorrectNamespacesRuleComplementDto> _action;

    public CorrectNamespacesRuleComplementDtoAssertion(Action<CorrectNamespacesRuleComplementDto> action)
    {
      _action = action;
    }

    public override void Visit(CorrectNamespacesRuleComplementDto dto) => _action(dto);
  }
}