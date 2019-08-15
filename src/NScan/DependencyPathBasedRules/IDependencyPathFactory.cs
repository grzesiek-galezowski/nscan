namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public interface IDependencyPathFactory
  {
    IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination);
  }
}