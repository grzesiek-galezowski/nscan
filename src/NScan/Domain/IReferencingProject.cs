namespace TddXt.NScan.Domain
{
  public interface IReferencingProject : IDependencyPathBasedRuleTarget
  {
    void AddReferencedProject(ProjectId projectId, IReferencedProject referencedProject);
    void ResolveReferencesFrom(ISolutionContext solution);
    bool IsRoot();
    void ResolveAsReferencing(IReferencedProject project);
  }
}