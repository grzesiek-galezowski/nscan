namespace MyTool.App
{
  public interface IReferencedProject : IProjectWithId
  {
    void Print(int nestingLevel);
    void AddReferencingProject(IReferencingProject referencingProject);
  }
}