namespace NScan.Domain.DependencyPathBasedRules
{
  public interface IDependencyPathFactory
  {
    IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination);
  }
}