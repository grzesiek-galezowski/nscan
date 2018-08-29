using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTool.App
{
  public class DotNetStandardSolution : ISolution, ISolutionContext
  {
    private readonly Dictionary<ProjectId, IDotNetProject> _projectMetadataByAbsolutePath;

    public DotNetStandardSolution(Dictionary<ProjectId, IDotNetProject> projectMetadataByAbsolutePath)
    {
      _projectMetadataByAbsolutePath = projectMetadataByAbsolutePath;
    }

    public void ResolveAllProjectsReferences()
    {
      foreach (var referencingProject in _projectMetadataByAbsolutePath.Values)
      {
        referencingProject.ResolveReferencesFrom(this);
      }
    }

    public void Print()
    {
      foreach (var project in _projectMetadataByAbsolutePath.Values.Where(v => v.IsRoot()))
      {
        project.Print(0);
      }
    }

    public void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId)
    {
      try
      {
        var referencedProject = _projectMetadataByAbsolutePath[referencedProjectId];

        referencingProject.AddReferencedProject(referencedProjectId, referencedProject);
        referencedProject.AddReferencingProject(referencingProject.Id, referencingProject);
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