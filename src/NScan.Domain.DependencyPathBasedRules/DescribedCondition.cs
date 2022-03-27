using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules;

public class DescribedCondition : IDescribedDependencyCondition
{
  private readonly IDependencyCondition _dependencyCondition;
  private readonly RuleDescription _description;

  public DescribedCondition(IDependencyCondition dependencyCondition, RuleDescription ruleDescription)
  {
    _dependencyCondition = dependencyCondition;
    _description = ruleDescription;
  }

  public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
  {
    return _dependencyCondition.Matches(depending, dependency);
  }

  public RuleDescription Description()
  {
    return _description;
  }
}