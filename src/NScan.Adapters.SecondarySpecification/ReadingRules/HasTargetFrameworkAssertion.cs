using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public class HasTargetFrameworkAssertion(Action<HasTargetFrameworkRuleComplementDto> action) : RuleUnionDtoAssertion
{
  public override void Visit(HasTargetFrameworkRuleComplementDto dto) => action(dto);
}
