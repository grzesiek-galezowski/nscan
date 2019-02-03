using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.Root
{
  public interface IDotNetProject : IReferencedProject, IReferencingProject, IProjectScopedRuleTarget, INamespaceBasedRuleTarget
  {
  }
}