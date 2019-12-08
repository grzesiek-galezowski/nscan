using NScan.Lib.Union1;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.SharedKernel.RuleDtos
{
  public class IndependentRuleNameExtractionVisitor :
    IUnionTransformingVisitor<
      IndependentRuleComplementDto,
      string>
  {
    public string Visit(IndependentRuleComplementDto dto)
    {
      return dto.RuleName;
    }
  }
}