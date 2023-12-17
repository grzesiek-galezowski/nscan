using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public class CorrectNamespacesRuleComplementDtoAssertion(Action<CorrectNamespacesRuleComplementDto> action)
  : RuleUnionDtoAssertion
{
  public override void Visit(CorrectNamespacesRuleComplementDto dto) => action(dto);
}
