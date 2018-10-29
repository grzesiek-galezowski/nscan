using Glob;
using TddXt.NScan.App;

namespace TddXt.NScan.CompositionRoot
{
  public class RuleFactory : IRuleFactory
  {
    public IDependencyRule CreateIndependentOfProjectRule(string dependingPattern, string dependencyPattern)
    {
      var dependencyAssemblyNamePattern = new Glob.Glob(dependencyPattern);
      var dependingAssemblyNamePattern = new Glob.Glob(dependingPattern);

      return new IndependentRule(
        new JoinedDescribedCondition(
          dependencyAssemblyNamePattern,
          dependingAssemblyNamePattern,
          new IsFollowingAssemblyCondition(), 
          new FollowingAssemblyMatchesPatternCondition(
            dependencyAssemblyNamePattern)), //todo refactor even higher the glob type?
        dependingAssemblyNamePattern);
    }

    public IDependencyRule CreateIndependentOfPackageRule(string dependingPattern, string packageNamePattern)
    {
      return new IndependentRule(
        new FollowingAssemblyHasPackageMatchingCondition(new Glob.Glob(packageNamePattern))
        , new Glob.Glob(dependingPattern));
    }
  }

  public class FollowingAssemblyHasPackageMatchingCondition : IDescribedDependencyCondition
  {
    public FollowingAssemblyHasPackageMatchingCondition(Glob.Glob glob)
    {
      
    }

    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      throw new System.NotImplementedException();
    }

    public string Description()
    {
      throw new System.NotImplementedException();
    }
  }
}