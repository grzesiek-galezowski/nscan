using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IDependencyPathFactory
  {
    IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination);
  }
}