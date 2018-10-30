using Glob;

namespace TddXt.NScan.CompositionRoot
{
  public class RuleFactory : IRuleFactory
  {
    public IDependencyRule CreateIndependentOfProjectRule(string dependingPattern, string dependencyPattern)
    {
      var dependencyAssemblyNamePattern = new Glob.Glob(dependencyPattern);
      var dependingAssemblyNamePattern = new Glob.Glob(dependingPattern);

      return new IndependentRule(
        new JoinedDescribedCondition(new IsFollowingAssemblyCondition(),
          new FollowingAssemblyMatchesPatternCondition(
            dependencyAssemblyNamePattern),
          DependencyDescriptions.IndependentOf(
            dependingAssemblyNamePattern.Pattern,
            dependencyAssemblyNamePattern.Pattern)), //todo refactor even higher the glob type?
        dependingAssemblyNamePattern);
    }

    public IDependencyRule CreateIndependentOfPackageRule(string dependingPattern, string packageNamePattern)
    {
      var dependingPatternGlob = new Glob.Glob(dependingPattern);
      var packagePatternGlob = new Glob.Glob(packageNamePattern);
      return new IndependentRule(
        new DescribedCondition(
          new HasPackageReferenceMatchingCondition(packagePatternGlob),
          DependencyDescriptions.IndependentOf(
            dependingPatternGlob.Pattern,
            "package:" + packagePatternGlob.Pattern)), dependingPatternGlob);
    }
  }
}