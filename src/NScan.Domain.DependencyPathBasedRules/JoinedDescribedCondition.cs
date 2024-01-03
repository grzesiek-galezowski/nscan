namespace NScan.DependencyPathBasedRules;

public interface IDependencyCondition
{
  bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency);
}

public interface IDescribedDependencyCondition : IDependencyCondition
{
  public RuleDescription Description();
}

public class JoinedDescribedCondition(
  IDependencyCondition condition1,
  IDependencyCondition condition2,
  RuleDescription conditionDescription)
  : IDescribedDependencyCondition
{
  public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
  {
    if (condition1.Matches(depending, dependency))
    {
      return condition2.Matches(depending, dependency);
    }
    return false;
  }

  public RuleDescription Description()
  {
    return conditionDescription;
  }

}
