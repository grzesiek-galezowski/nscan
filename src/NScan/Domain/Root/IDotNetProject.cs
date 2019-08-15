using NScan.Domain.Domain.DependencyPathBasedRules;
using NScan.Domain.Domain.NamespaceBasedRules;
using NScan.Domain.Domain.ProjectScopedRules;

namespace NScan.Domain.Domain.Root
{
  public interface IDotNetProject : IReferencedProject, IReferencingProject, IProjectScopedRuleTarget, INamespaceBasedRuleTarget, IDependencyPathBasedRuleTarget
  {}
}