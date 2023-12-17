using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public class NoUsingsRuleComplementDtoAssertion(Action<NoUsingsRuleComplementDto> action) : RuleUnionDtoAssertion
{
  public override void Visit(NoUsingsRuleComplementDto dto)
  {
    action(dto);
  }
}
