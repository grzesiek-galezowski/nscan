using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class RuleFactory : IRuleFactory
  {
    public IDependencyRule CreateIndependentOfProjectRule(ProjectId dependingId, ProjectId dependencyId)
    {
      return new DirectIndependentOfProjectRule(dependingId, dependencyId);
    }
  }
}