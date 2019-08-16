using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public interface ISolutionContext
  {
    void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId);
  }
}