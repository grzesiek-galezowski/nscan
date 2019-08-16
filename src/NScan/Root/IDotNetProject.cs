using NScan.Domain.DependencyPathBasedRules;
using NScan.Domain.NamespaceBasedRules;
using NScan.Domain.ProjectScopedRules;

namespace NScan.Domain.Root
{
  public interface IDotNetProject : IReferencedProject, IReferencingProject, IProjectScopedRuleTarget, INamespaceBasedRuleTarget, IDependencyPathBasedRuleTarget
  {}
}