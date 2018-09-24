namespace NScanRoot.CompositionRoot
{
  public class RuleFactory : IRuleFactory
  {
    public IDependencyRule CreateIndependentOfProjectRule(string dependingId, string dependencyId)
    {
      return new IndependentOfProjectRule(dependingId, dependencyId);
    }
  }
}