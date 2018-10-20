namespace TddXt.NScan.App
{
  public interface ISolutionContext
  {
    void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId);
  }
}