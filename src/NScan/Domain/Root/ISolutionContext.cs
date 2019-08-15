using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.Root
{
  public interface ISolutionContext
  {
    void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId);
  }
}