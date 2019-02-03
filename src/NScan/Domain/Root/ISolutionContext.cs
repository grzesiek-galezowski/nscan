using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.Root
{
  public interface ISolutionContext
  {
    void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId);
  }
}