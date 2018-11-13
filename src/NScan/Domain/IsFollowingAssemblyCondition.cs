namespace TddXt.NScan.Domain
{
  public class IsFollowingAssemblyCondition : IDependencyCondition
  {
    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return depending.IsNot(dependency);
    }
  }
}