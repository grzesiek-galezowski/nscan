using MyTool.App;

namespace MyTool.CompositionRoot
{
  public interface IRuleFactory
  {
    IDependencyRule CreateIndependentOfProjectRule(string dependingId, string dependencyId);
  }
}