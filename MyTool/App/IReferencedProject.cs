namespace MyTool.App
{
  public interface IReferencedProject
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId referencingProjectId, IReferencingProject referencingProject);
  }
}