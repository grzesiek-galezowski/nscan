using NScan.Lib.Union1;

namespace NScan.SharedKernel.RuleDtos.NamespaceBased
{
  public class NamespaceBasedRuleUnionDto : Union<NoCircularUsingsRuleComplementDto>
  {
    private readonly IUnionTransformingVisitor<
      NoCircularUsingsRuleComplementDto, 
      string> _ruleNameExtractionVisitor = new RuleNameExtractionVisitor();

    public static NamespaceBasedRuleUnionDto With(NoCircularUsingsRuleComplementDto dto)
    {
      return new NamespaceBasedRuleUnionDto(dto);
    }

    public string RuleName => Accept(_ruleNameExtractionVisitor);

    private NamespaceBasedRuleUnionDto(NoCircularUsingsRuleComplementDto o) : base(o)
    {
    }

  }
}