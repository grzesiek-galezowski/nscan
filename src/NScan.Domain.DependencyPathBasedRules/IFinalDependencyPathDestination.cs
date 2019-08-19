namespace NScan.Domain.DependencyPathBasedRules
{
  public interface IFinalDependencyPathDestination
  {
    void Add(IProjectDependencyPath projectDependencyPath);
  }
}