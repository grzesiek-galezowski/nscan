using NScan.SharedKernel.SharedKernel;
using TddXt.NScan.Domain.DependencyPathBasedRules;

namespace TddXt.NScan.Domain.Root
{
  public interface IReferencedProject : IDependencyPathBasedRuleTarget
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
  }
}