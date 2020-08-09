using System.Collections.Generic;
using System.Linq;
using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;

namespace NScan.Domain
{
  public class DotNetStandardSolution : ISolution, ISolutionContext
  {
    private readonly IReadOnlyDictionary<ProjectId, IDotNetProject> _projectsById;

    public DotNetStandardSolution(
      IReadOnlyDictionary<ProjectId, IDotNetProject> projectsById)
    {
      _projectsById = projectsById;
    }

    public void ResolveAllProjectsReferences()
    {
      //backlog use the analysis report to write what projects are skipped - write a separate acceptance test for that
      foreach (var referencingProject in _projectsById.Values)
      {
        referencingProject.ResolveReferencesFrom(this);
      }
    }

    public void PrintDebugInfo()
    {
      foreach (var project in _projectsById.Values.Where(v => v.IsRoot()))
      {
        project.Print(0);
      }
    }

    public void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId)
    {
      try
      {
        var referencedProject = _projectsById[referencedProjectId];

        referencingProject.ResolveAsReferencing(referencedProject);
        referencedProject.ResolveAsReferenceOf(referencingProject);
      }
      catch (KeyNotFoundException e)
      {
        throw new ReferencedProjectNotFoundInSolutionException(
          CouldNotFindProjectFor(referencedProjectId), e);
      }
    }

    private static string CouldNotFindProjectFor(ProjectId referencedProjectId)
    {
      return $"Could not find referenced project {referencedProjectId} " +
             "probably because it was in an incompatible format and was skipped during project collection phase.";
    }
  }
}