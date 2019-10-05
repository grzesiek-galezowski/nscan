using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;

namespace NScan.Domain.Root
{
  public interface IDotNetProject : IReferencedProject, IReferencingProject, IProjectScopedRuleTarget, INamespaceBasedRuleTarget, IDependencyPathBasedRuleTarget
  {}
}