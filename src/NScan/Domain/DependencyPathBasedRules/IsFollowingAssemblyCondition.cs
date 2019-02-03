using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public class IsFollowingAssemblyCondition : IDependencyCondition
  {
    public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
    {
      return depending.IsNot(dependency);
    }
  }
}