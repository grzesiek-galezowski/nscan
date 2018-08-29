using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTool.App
{
  public interface ISolution
  {
    void ResolveAllProjectsReferences();
    void Print();
  }

  public interface ISolutionContext
  {
    void ResolveReferenceFor(DotNetStandardProject referencingCsProject, ProjectId referencedProjectPath);
  }

  public class DotNetStandardSolution : ISolution, ISolutionContext
  {
    private readonly Dictionary<ProjectId, DotNetStandardProject> _projectMetadataByAbsolutePath;

    public DotNetStandardSolution(Dictionary<ProjectId, DotNetStandardProject> projectMetadataByAbsolutePath)
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

    public void ResolveReferenceFor(DotNetStandardProject referencingCsProject, ProjectId referencedProjectPath)
    {
      try
      {
        var referencedProject = _projectMetadataByAbsolutePath[referencedProjectPath];

        referencingCsProject.AddReferencedProject(referencedProjectPath, referencedProject);
        referencedProject.AddReferencingProject(referencingCsProject.Id, referencingCsProject);
      }
      catch (KeyNotFoundException e)
      {
        Console.WriteLine(
          $"Could not find referenced project {referencedProjectPath} " +
          "probably because it was not in a compatible format and was skipped during project collection phase");
      }
    }
  }
}