using System;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapter.ReadingRulesSpecification
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
