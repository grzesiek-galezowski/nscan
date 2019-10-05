using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public interface IReferencingProjects
  {
    void Put(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject);
    bool AreEmpty();
  }

  public class ReferencingProjects : IReferencingProjects
  {
    private readonly IDictionary<ProjectId, IDependencyPathBasedRuleTarget> _referencingProjects 
      = new Dictionary<ProjectId, IDependencyPathBasedRuleTarget>();

    public void Put(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject)
    {
      AssertThisIsAddingTheSameReferenceNotShadowing(projectId, referencingProject);
      _referencingProjects[projectId] = referencingProject;
    }

    public bool AreEmpty()
    {
      return !_referencingProjects.Any();
    }

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private void AssertThisIsAddingTheSameReferenceNotShadowing(
      ProjectId referencingProjectId,
      IDependencyPathBasedRuleTarget referencingProject)
    {
      if (_referencingProjects.ContainsKey(referencingProjectId)
          && !_referencingProjects[referencingProjectId].Equals(referencingProject))
      {
        throw new ProjectShadowingException(_referencingProjects[referencingProjectId], referencingProject);
      }
    }
  }
}