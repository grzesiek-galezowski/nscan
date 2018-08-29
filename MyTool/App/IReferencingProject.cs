namespace MyTool.App
{
  public interface IReferencingProject
  {
    void AddReferencedProject(ProjectId referencedProjectId, IReferencedProject referencedProject);
    ProjectId Id { get; }
    void ResolveReferencesFrom(ISolutionContext dotNetStandardSolution);
    bool IsRoot();
  }
}