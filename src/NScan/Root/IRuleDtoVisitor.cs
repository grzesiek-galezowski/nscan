using NScan.Lib;
using NScan.SharedKernel.RuleDtos;

namespace NScan.Domain.Root
{
  public interface IRuleDtoVisitor : IUnion5Visitor<
    IndependentRuleComplementDto,
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto
  >
  {
  }
}