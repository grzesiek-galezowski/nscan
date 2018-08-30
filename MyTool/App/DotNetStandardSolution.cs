using System;
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

        referencingProject.AddReferencedProject(referencedProject);
        referencedProject.AddReferencingProject(referencingProject);
      }
      catch (KeyNotFoundException e)
      {
        Console.WriteLine(
          $"Could not find referenced project {referencedProjectId} " +
          "probably because it was not in a compatible format and was skipped during project collection phase. Details: " + e);
      }
    }
  }
}