using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IFinalDependencyPathDestination
  {
    void Add(IProjectDependencyPath projectDependencyPath);
  }
}