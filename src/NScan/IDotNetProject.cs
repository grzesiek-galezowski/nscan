using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;

namespace NScan.Domain
{
  public interface IDotNetProject : 
    IReferencedProject, 
    IReferencingProject, 
    IProjectScopedRuleTarget, 
    INamespaceBasedRuleTarget, 
    IDependencyPathBasedRuleTarget
  {}
}