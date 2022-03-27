namespace NScan.DependencyPathBasedRules;

public interface IDependencyPathFactory
{
  IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination);
}