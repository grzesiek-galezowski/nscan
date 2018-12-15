using TddXt.NScan.Domain;

namespace TddXt.NScan.App
{
  public interface IDependencyPathInProgress
  {
    IDependencyPathInProgress CloneWith(IReferencedProject project);
    void FinalizeWith(IReferencedProject finalProject);
  }
}