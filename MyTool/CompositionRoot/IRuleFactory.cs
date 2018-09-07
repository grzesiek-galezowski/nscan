using MyTool.App;

namespace MyTool.CompositionRoot
{
  public interface IRuleFactory
  {
    IDependencyRule CreateDirectIndependentOfProjectRule(ProjectId dependingId, ProjectId dependencyId);
  }
}