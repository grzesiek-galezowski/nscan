using NScan.Lib.Union1;

namespace NScan.SharedKernel.RuleDtos.DependencyPathBased
{
  public sealed class DependencyPathBasedRuleUnionDto : Union<
    IndependentRuleComplementDto>
  {
    private readonly IUnionTransformingVisitor<IndependentRuleComplementDto, string> _ruleNameExtractionVisitor 
      = new IndependentRuleNameExtractionVisitor();

    public static DependencyPathBasedRuleUnionDto With(IndependentRuleComplementDto dto)
    {
      return new(dto);
    }

    public string RuleName => Accept(_ruleNameExtractionVisitor);

    private DependencyPathBasedRuleUnionDto(IndependentRuleComplementDto o) : base(o)
    {
    }
  }
}
