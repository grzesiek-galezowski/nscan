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

    public void ResolveAllProjectsReferences(IAnalysisInProgressReport analysisInProgressReport)
    {
      //bug use the analysis report to write what projects are skipped - write a separate acceptance test for that
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

    public void Check(IPathRuleSet pathRuleSet, IAnalysisInProgressReport analysisInProgressReport)
    {
      throw new System.NotImplementedException();
    }

    public void BuildCaches()
    {
      throw new System.NotImplementedException();
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