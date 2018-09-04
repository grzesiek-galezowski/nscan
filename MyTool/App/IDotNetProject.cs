namespace MyTool.App
{
  public interface IDotNetProject : IReferencedProject, IReferencingProject
  {
    void Accept(IDependencyPathInProgress dependencyStartingPathInProgress);
  }
}