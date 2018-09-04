using MyTool.App;

namespace MyTool
{
  public interface IDependencyPathFactory
  {
    IDependencyPathInProgress CreateNewDependencyPathFor(IDependencyPathDestination dependencyPathDestination);
  }
}