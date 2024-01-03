namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public class HasPropertyAssertion(Action<HasPropertyRuleComplementDto> action) : RuleUnionDtoAssertion
{
  public override void Visit(HasPropertyRuleComplementDto dto) => action(dto);
}
