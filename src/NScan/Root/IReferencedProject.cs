using NScan.Domain.DependencyPathBasedRules;
using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public interface IReferencedProject : IDependencyPathBasedRuleTarget
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
  }
}