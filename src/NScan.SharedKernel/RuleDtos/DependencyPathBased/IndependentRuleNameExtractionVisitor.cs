using NScan.Lib.Union1;

namespace NScan.SharedKernel.RuleDtos.DependencyPathBased;

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