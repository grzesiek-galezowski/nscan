namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public class HasAttributesOnRuleComplementAssertion(Action<HasAttributesOnRuleComplementDto> action)
  : RuleUnionDtoAssertion
{
  public override void Visit(HasAttributesOnRuleComplementDto dto)
  {
    action(dto);
  }
}
