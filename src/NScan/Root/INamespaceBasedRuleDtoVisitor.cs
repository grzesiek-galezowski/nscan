using NScan.Lib.Union2;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.Domain.Root
{
  public interface INamespaceBasedRuleDtoVisitor : IUnionVisitor<NoCircularUsingsRuleComplementDto, NoUsingsRuleComplementDto>
  {
  }
}