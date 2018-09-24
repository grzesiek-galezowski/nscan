namespace NScanRoot.CompositionRoot
{
  public interface IRuleFactory
  {
    IDependencyRule CreateIndependentOfProjectRule(string dependingId, string dependencyId);
  }
}