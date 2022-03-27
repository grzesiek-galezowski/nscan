using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.DependencyPathBasedRules;

public interface IDependencyBasedRuleFactory
{
  IDependencyRule CreateDependencyRuleFrom(IndependentRuleComplementDto independentRuleComplementDto);
}