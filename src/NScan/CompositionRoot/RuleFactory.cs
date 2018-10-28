namespace TddXt.NScan.CompositionRoot
{
  public class RuleFactory : IRuleFactory
  {
    public IDependencyRule CreateIndependentOfProjectRule(string dependingId, string dependencyId)
    {
      return new IndependentRule(dependingId, 
        new NextAssemblyMatchesPatternCondition(dependencyId));
    }

    public IDependencyRule CreateIndependentOfPackageRule(string dependingId, string dependencyId)
    {
      return new IndependentOfPackageRule(dependingId, dependencyId);
    }
  }
}