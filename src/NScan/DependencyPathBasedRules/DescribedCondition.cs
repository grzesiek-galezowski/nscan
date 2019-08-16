namespace NScan.Domain.DependencyPathBasedRules
{
  public class DescribedCondition : IDescribedDependencyCondition
  {
    private readonly IDependencyCondition _dependencyCondition;
    private readonly string _description;

    public DescribedCondition(IDependencyCondition dependencyCondition, string description)
    {
      _dependencyCondition = dependencyCondition;
      _description = description;
    }

    public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
    {
      return _dependencyCondition.Matches(depending, dependency);
    }

    public string Description()
    {
      return _description;
    }
  }
}