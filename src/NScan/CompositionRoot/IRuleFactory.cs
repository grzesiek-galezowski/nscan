namespace TddXt.NScan.CompositionRoot
{
  public interface IRuleFactory
  {
    IDependencyRule CreateIndependentOfProjectRule(string dependingId, string dependencyId);
    IDependencyRule CreateIndependentOfPackageRule(string dependingId, string dependencyId);
  }
}