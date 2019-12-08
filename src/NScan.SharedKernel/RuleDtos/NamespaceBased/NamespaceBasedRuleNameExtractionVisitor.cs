using NScan.Lib.Union2;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.SharedKernel.RuleDtos
{
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
}