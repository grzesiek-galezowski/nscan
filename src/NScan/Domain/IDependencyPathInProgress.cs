namespace TddXt.NScan.Domain
{
  public interface IDependencyPathInProgress
  {
    IDependencyPathInProgress CloneWith(IReferencedProject project);
    void FinalizeWith(IReferencedProject finalProject);
  }
}