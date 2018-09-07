using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class RuleFactory : IRuleFactory
  {
    public IDependencyRule CreateDirectIndependentOfProjectRule(ProjectId dependingId, ProjectId dependencyId)
    {
      throw new System.NotImplementedException();
    }
  }
}