namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public class NoCircularUsingsRuleComplementDtoAssertion(Action<NoCircularUsingsRuleComplementDto> action)
  : RuleUnionDtoAssertion
{
  public override void Visit(NoCircularUsingsRuleComplementDto dto)
  {
    action(dto);
  }
}
