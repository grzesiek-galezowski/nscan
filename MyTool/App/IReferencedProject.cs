namespace MyTool.App
{
  public interface IReferencedProject
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IReferencingProject referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
    void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress);
  }
}