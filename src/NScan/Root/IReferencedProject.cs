using NScan.Domain.Domain.DependencyPathBasedRules;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.Root
{
  public interface IReferencedProject : IDependencyPathBasedRuleTarget
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
  }
}