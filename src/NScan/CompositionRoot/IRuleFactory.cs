using GlobExpressions;

namespace TddXt.NScan.CompositionRoot
{
  public interface IRuleFactory
  {
    IDependencyRule CreateIndependentOfProjectRule(Glob dependingNamePattern, Glob dependencyNamePattern);
    IDependencyRule CreateIndependentOfPackageRule(Glob dependingNamePattern, Glob packageNamePattern);
    IDependencyRule CreateIndependentOfAssemblyRule(Glob dependingNamePattern, Glob assemblyNamePattern);
  }
}