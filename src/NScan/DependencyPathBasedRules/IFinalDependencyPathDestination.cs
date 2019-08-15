namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public interface IFinalDependencyPathDestination
  {
    void Add(IProjectDependencyPath projectDependencyPath);
  }
}