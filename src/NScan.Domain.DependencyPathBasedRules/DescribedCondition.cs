using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules;

public class DescribedCondition(IDependencyCondition dependencyCondition, RuleDescription ruleDescription)
  : IDescribedDependencyCondition
{
  public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
  {
    return dependencyCondition.Matches(depending, dependency);
  }

  public RuleDescription Description()
  {
    return ruleDescription;
  }
}
