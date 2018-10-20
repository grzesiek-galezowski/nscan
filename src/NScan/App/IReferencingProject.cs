namespace TddXt.NScan.App
{
  public interface IReferencingProject
  {
    void AddReferencedProject(ProjectId projectId, IReferencedProject referencedProject);
    void ResolveReferencesFrom(ISolutionContext dotNetStandardSolution);
    bool IsRoot();
    void ResolveAsReferencing(IReferencedProject project);
  }
}