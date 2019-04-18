namespace TddXt.NScan.ReadingRules.Ports
{
  public class RuleUnionDto : Union3<
    IndependentRuleComplementDto, 
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto>
  {
    public static RuleUnionDto With(CorrectNamespacesRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }

    public static RuleUnionDto With(NoCircularUsingsRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }

    public static RuleUnionDto With(IndependentRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }

    public string RuleName => Match(
      dto => dto.RuleName,
      dto => dto.RuleName,
      dto => dto.RuleName);

    private RuleUnionDto(IndependentRuleComplementDto o) : base(o) {}
    private RuleUnionDto(CorrectNamespacesRuleComplementDto o) : base(o) {}
    private RuleUnionDto(NoCircularUsingsRuleComplementDto o) : base(o) {}
  }
}