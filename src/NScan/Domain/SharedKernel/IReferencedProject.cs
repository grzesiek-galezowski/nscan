using TddXt.NScan.Domain.DependencyPathBasedRules;

namespace TddXt.NScan.Domain.SharedKernel
{
  public interface IReferencedProject : IDependencyPathBasedRuleTarget
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
  }
}