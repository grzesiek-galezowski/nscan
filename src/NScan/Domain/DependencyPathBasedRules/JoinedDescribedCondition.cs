namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public interface IDependencyCondition
  {
    bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency);
  }

  public interface IDescribedDependencyCondition : IDependencyCondition
  {
    string Description();
  }

  public class JoinedDescribedCondition : IDescribedDependencyCondition
  {
    private readonly IDependencyCondition _condition1;
    private readonly IDependencyCondition _condition2;
    private readonly string _conditionDescription;

    public JoinedDescribedCondition(
      IDependencyCondition condition1,
      IDependencyCondition condition2,
      string conditionDescription)
    {
      _condition1 = condition1;
      _condition2 = condition2;
      _conditionDescription = conditionDescription;
    }

    public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
    {
      if (_condition1.Matches(depending, dependency))
      {
        return _condition2.Matches(depending, dependency);
      }
      return false;
    }

    public string Description()
    {
      return _conditionDescription;
    }
  }
}