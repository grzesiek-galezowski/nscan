using NScan.Lib.Union2;

namespace NScan.SharedKernel.RuleDtos.NamespaceBased;

public class NamespaceBasedRuleNameExtractionVisitor :
  IUnionTransformingVisitor<
    NoCircularUsingsRuleComplementDto,
    NoUsingsRuleComplementDto,
    string>
{
  public string Visit(NoCircularUsingsRuleComplementDto dto)
  {
    return dto.RuleName;
  }

  public string Visit(NoUsingsRuleComplementDto dto)
  {
    return dto.RuleName;
  }
}