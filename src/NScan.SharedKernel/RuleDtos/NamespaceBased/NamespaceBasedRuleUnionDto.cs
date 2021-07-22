using NScan.Lib.Union2;

namespace NScan.SharedKernel.RuleDtos.NamespaceBased
{
  public class NamespaceBasedRuleUnionDto : Union<
    NoCircularUsingsRuleComplementDto,
    NoUsingsRuleComplementDto
  >
  {
    private readonly IUnionTransformingVisitor<
      NoCircularUsingsRuleComplementDto, 
      NoUsingsRuleComplementDto, 
      string> _ruleNameExtractionVisitor = new NamespaceBasedRuleNameExtractionVisitor();

    public static NamespaceBasedRuleUnionDto With(NoCircularUsingsRuleComplementDto dto)
    {
      return new NamespaceBasedRuleUnionDto(dto);
    }

    public static NamespaceBasedRuleUnionDto With(NoUsingsRuleComplementDto dto)
    {
      return new NamespaceBasedRuleUnionDto(dto);
    }

    public string RuleName => Accept(_ruleNameExtractionVisitor);

    private NamespaceBasedRuleUnionDto(NoCircularUsingsRuleComplementDto o) : base(o)
    {
    }
    private NamespaceBasedRuleUnionDto(NoUsingsRuleComplementDto o) : base(o)
    {
    }

  }
}
