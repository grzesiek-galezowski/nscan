using System.Collections.Generic;
using System.Linq;

namespace NScanRoot.App
{
  public class DotNetStandardSolution : ISolution, ISolutionContext
  {
    private readonly Dictionary<ProjectId, IDotNetProject> _projectsById;
    private readonly IPathCache _pathCache;

    public DotNetStandardSolution(
      Dictionary<ProjectId, IDotNetProject> projectsById, 
      IPathCache pathCache)
    {
      _projectsById = projectsById;
      _pathCache = pathCache;
    }

    public void ResolveAllProjectsReferences(IAnalysisReportInProgress analysisReportInProgress)
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

    public void Check(IPathRuleSet pathRuleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      pathRuleSet.Check(_pathCache, analysisReportInProgress);
    }

    public void BuildCache()
    {
      _pathCache.BuildStartingFrom(RootProjects());
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

    private IDotNetProject[] RootProjects()
    {
      return _projectsById.Values.Where(project => project.IsRoot()).ToArray();
    }
  }
}