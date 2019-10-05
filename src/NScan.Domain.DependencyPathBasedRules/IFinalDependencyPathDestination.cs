namespace NScan.DependencyPathBasedRules
{
  public interface IFinalDependencyPathDestination
  {
    void Add(IProjectDependencyPath projectDependencyPath);
  }
}