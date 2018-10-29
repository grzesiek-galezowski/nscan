namespace TddXt.NScan.CompositionRoot
{
  public interface IRuleFactory
  {
    IDependencyRule CreateIndependentOfProjectRule(string dependingPattern, string dependencyPattern);
    IDependencyRule CreateIndependentOfPackageRule(string dependingPattern, string packageNamePattern);
  }
}