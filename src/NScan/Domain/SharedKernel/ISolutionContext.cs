using TddXt.NScan.Domain.DependencyPathBasedRules;

namespace TddXt.NScan.Domain.SharedKernel
{
  public interface ISolutionContext
  {
    void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId);
  }
}