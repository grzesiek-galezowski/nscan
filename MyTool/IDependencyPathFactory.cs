using MyTool.App;

namespace MyTool
{
  public interface IDependencyPathFactory
  {
    IDependencyPath CreateNewDependencyPathFor(IDependencyPathDestination dependencyPathDestination);
  }
}