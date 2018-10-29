using TddXt.NScan.App;

namespace TddXt.NScan
{
  public class IsFollowingAssemblyCondition : IDependencyCondition
  {
    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return depending.IsNot(dependency);
    }
  }
}