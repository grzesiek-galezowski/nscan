using NScan.DependencyPathBasedRules;
using NScan.ProjectScopedRules;

namespace NScan.Domain
{
  public interface IDotNetProject : 
    IReferencedProject,
    IReferencingProject, 
    IProjectScopedRuleTarget, 
    IDependencyPathBasedRuleTarget
  {}
}