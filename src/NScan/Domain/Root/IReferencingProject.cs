using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.Root
{
  public interface IReferencingProject
  {
    void AddReferencedProject(ProjectId projectId, IReferencedProject referencedProject);
    void ResolveReferencesFrom(ISolutionContext solution);
    bool IsRoot();
    void ResolveAsReferencing(IReferencedProject project);
  }
}