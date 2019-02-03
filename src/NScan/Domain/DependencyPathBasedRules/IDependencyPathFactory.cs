namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IDependencyPathFactory
  {
    IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination);
  }
}