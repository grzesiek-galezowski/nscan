namespace TddXt.NScan.Domain
{
  public interface IDependencyPathBasedRuleTarget
  {
    void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress);
  }
}