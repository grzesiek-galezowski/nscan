namespace NScan.DependencyPathBasedRules;

public interface IReferencedProject
{
  void Print(int nestingLevel);
  void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject);
  void ResolveAsReferenceOf(IReferencingProject project);
  void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress);
}