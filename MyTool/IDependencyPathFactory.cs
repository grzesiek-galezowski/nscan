using MyTool.App;

namespace MyTool
{
  public interface IDependencyPathFactory
  {
    IDependencyPathInProgress CreateNewDependencyPathFor(IFinalDependencyPathDestination destination);
  }
}