using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public class IndependentRuleComplementDtoAssertion(
  Action<IndependentRuleComplementDto> action) : RuleUnionDtoAssertion
{
  public override void Visit(IndependentRuleComplementDto dto)
  {
    action(dto);
  }
}
