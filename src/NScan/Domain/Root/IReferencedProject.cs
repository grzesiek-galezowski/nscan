using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.Root
{
  public interface IReferencedProject : IDependencyPathBasedRuleTarget
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
  }
}