using NScan.Lib.Union1;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.Domain.Root
{
  public interface IPathBasedRuleDtoVisitor : IUnionVisitor<IndependentRuleComplementDto>
  {
  }
}