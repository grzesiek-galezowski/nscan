using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;

namespace TddXt.NScan.Domain.Root
{
  public interface IDotNetProject : IReferencedProject, IReferencingProject, IProjectScopedRuleTarget, INamespaceBasedRuleTarget, IDependencyPathBasedRuleTarget
  {}
}