namespace TddXt.NScan.App
{
  public interface IReferencedProject
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IReferencingProject referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
    void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress);
    bool HasAssemblyNameMatching(string assemblyNamePattern);

    string ToString();
  }
}