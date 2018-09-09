using MyTool.App;

namespace MyTool.CompositionRoot
{
  public interface IRuleFactory
  {
    IDependencyRule CreateIndependentOfProjectRule(ProjectId dependingId, ProjectId dependencyId);
  }
}