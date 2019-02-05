using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.Root
{
  public class ReferencingProjects
  {
    private readonly IDictionary<ProjectId, IDependencyPathBasedRuleTarget> _referencingProjects 
      = new Dictionary<ProjectId, IDependencyPathBasedRuleTarget>();

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    public void AssertThisIsAddingTheSameReferenceNotShadowing(
      ProjectId referencingProjectId,
      IDependencyPathBasedRuleTarget referencingProject)
    {
      if (_referencingProjects.ContainsKey(referencingProjectId)
          && !_referencingProjects[referencingProjectId].Equals(referencingProject))
      {
        throw new ProjectShadowingException(_referencingProjects[referencingProjectId], referencingProject);
      }
    }

    public void Put(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject)
    {
      AssertThisIsAddingTheSameReferenceNotShadowing(projectId, referencingProject);
      _referencingProjects[projectId] = referencingProject;
    }

    public bool AreEmpty()
    {
      return !_referencingProjects.Any();
    }
  }
}