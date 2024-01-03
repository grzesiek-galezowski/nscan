namespace NScan.DependencyPathBasedRules;

public interface ISolutionContext
{
  void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId);
}