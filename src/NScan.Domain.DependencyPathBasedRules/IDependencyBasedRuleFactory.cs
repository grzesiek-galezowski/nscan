using NScan.DependencyPathBasedRules;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.Domain.Root
{
  public interface IDependencyBasedRuleFactory
  {
    IDependencyRule CreateDependencyRuleFrom(IndependentRuleComplementDto independentRuleComplementDto);
  }
}