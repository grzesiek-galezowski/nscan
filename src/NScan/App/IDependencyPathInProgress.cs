namespace TddXt.NScan.App
{
  public interface IDependencyPathInProgress
  {
    IDependencyPathInProgress CloneWith(IReferencedProject project); //TODO maybe a specific interface instead of IDotNetProject?
    void FinalizeWith(IReferencedProject finalProject); //TODO maybe a specific interface?
  }
}