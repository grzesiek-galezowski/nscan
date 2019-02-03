using TddXt.NScan.Domain.DependencyPathBasedRules;

namespace TddXt.NScan.Domain.SharedKernel
{
  public interface IReferencingProject : IDependencyPathBasedRuleTarget
  {
    void AddReferencedProject(ProjectId projectId, IReferencedProject referencedProject);
    void ResolveReferencesFrom(ISolutionContext solution);
    bool IsRoot();
    void ResolveAsReferencing(IReferencedProject project);
  }
}