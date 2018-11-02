
using GlobExpressions;

namespace TddXt.NScan.CompositionRoot
{
  public class RuleFactory : IRuleFactory
  {
    public IDependencyRule CreateIndependentOfProjectRule(Glob dependingNamePattern, Glob dependencyNamePattern)
    {
      var dependencyAssemblyNamePattern = dependencyNamePattern;
      var dependingAssemblyNamePattern = dependingNamePattern;

      return new IndependentRule(
        new JoinedDescribedCondition(new IsFollowingAssemblyCondition(),
          new FollowingAssemblyMatchesPatternCondition(
            dependencyAssemblyNamePattern),
          DependencyDescriptions.IndependentOf(
            dependingAssemblyNamePattern.Pattern,
            dependencyAssemblyNamePattern.Pattern)),
        dependingAssemblyNamePattern);
    }

    public IDependencyRule CreateIndependentOfPackageRule(Glob dependingNamePattern, Glob packageNamePattern)
    {
      return new IndependentRule(
        new DescribedCondition(
          new HasPackageReferenceMatchingCondition(packageNamePattern),
          DependencyDescriptions.IndependentOf(
            dependingNamePattern.Pattern,
            "package:" + packageNamePattern.Pattern)), dependingNamePattern);
    }

    public IDependencyRule CreateIndependentOfAssemblyRule(Glob dependingNamePattern, Glob assemblyNamePattern)
    {
      return new IndependentRule(new DescribedCondition(new HasAssemblyReferenceMatchingCondition(assemblyNamePattern), 
        DependencyDescriptions.IndependentOf(dependingNamePattern.Pattern,
          "assembly:" + assemblyNamePattern.Pattern)), dependingNamePattern);
    }
  }
}