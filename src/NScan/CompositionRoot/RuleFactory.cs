namespace TddXt.NScan.CompositionRoot
{
  public class RuleFactory : IRuleFactory
  {
    public IDependencyRule CreateIndependentOfProjectRule(string dependingId, string dependencyId)
    {
      return new IndependentRule(
        new NextAssemblyMatchesPatternCondition(
          new Glob.Glob(dependencyId)),
        new Glob.Glob(dependingId));
    }

    public IDependencyRule CreateIndependentOfPackageRule(string dependingId, string dependencyId)
    {
      return new IndependentOfPackageRule(dependingId, dependencyId);
    }
  }
}