
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
            dependencyAssemblyNamePattern.Pattern)), //todo refactor even higher the glob type?
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
  }
}