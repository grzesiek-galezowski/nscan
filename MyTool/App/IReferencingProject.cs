namespace MyTool.App
{
  public interface IReferencingProject : IProjectWithId
  {
    void AddReferencedProject(IReferencedProject referencedProject);
    void ResolveReferencesFrom(ISolutionContext dotNetStandardSolution);
    bool IsRoot();
  }
}