using NScan.Lib;
using NScan.Lib.Union5;
using NScan.SharedKernel.RuleDtos;

namespace NScan.Domain.Root
{
  public interface IRuleDtoVisitor : IUnionVisitor<
    IndependentRuleComplementDto,
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto
  >
  {
  }
}