using System;
using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules
{
  public class SolutionForDependencyPathRules 
    : ISolutionForDependencyPathBasedRules, ISolutionContext
  {
    private readonly IPathCache _pathCache;
    private readonly IReadOnlyDictionary<ProjectId, IDotNetProject> _projectsById;

    public SolutionForDependencyPathRules(
      IPathCache pathCache,
      IReadOnlyDictionary<ProjectId, IDotNetProject> projectsById)
    {
      _pathCache = pathCache;
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
          CouldNotFindProjectFor(referencedProjectId, _projectsById), e);
      }
    }

    private static string CouldNotFindProjectFor(ProjectId referencedProjectId,
      IReadOnlyDictionary<ProjectId, IDotNetProject> projectsById)
    {
      return
        $"Could not find referenced project {referencedProjectId} " +
        "probably because it was in an incompatible format " +
        "and was skipped during project collection phase. " +
        "Existing project keys: " +
        $"{string.Join(Environment.NewLine + "* ", projectsById.Keys)}";
    }

    public void Check(IPathRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      ruleSet.Check(_pathCache, analysisReportInProgress);
    }

    public void BuildDependencyPathCache()
    {
      _pathCache.BuildStartingFrom(RootProjects());
    }

    private IDotNetProject[] RootProjects()
    {
      return Projects().Where(project => project.IsRoot()).ToArray();
    }

    private IReadOnlyList<IDotNetProject> Projects()
    {
      return _projectsById.Values.ToList();
    }

  }
}
