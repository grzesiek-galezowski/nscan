using System.Collections.Generic;
using System.Linq;

namespace MyTool.App
{
  public class DotNetStandardSolution : ISolution, ISolutionContext
  {
    private readonly Dictionary<ProjectId, IDotNetProject> _projectsById;

    public DotNetStandardSolution(Dictionary<ProjectId, IDotNetProject> projectsById)
    {
      _projectsById = projectsById;
    }

    public void ResolveAllProjectsReferences()
    {
      foreach (var referencingProject in _projectsById.Values)
      {
        referencingProject.ResolveReferencesFrom(this);
      }
    }

    public void Print()
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
          $"Could not find referenced project {referencedProjectId} " +
          "probably because it was in an incompatible format and was skipped during project collection phase.", e);
      }
    }
  }
}