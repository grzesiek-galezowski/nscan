﻿using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.App;

namespace TddXt.NScan.Domain
{
  public class DotNetStandardSolution : ISolution, ISolutionContext
  {
    private readonly Dictionary<ProjectId, IDotNetProject> _projectsById;
    private readonly IPathCache _pathCache;

    public DotNetStandardSolution(Dictionary<ProjectId, IDotNetProject> projectsById,
      IPathCache pathCache, INamespacesCache namespacesCache)
    {
      _projectsById = projectsById;
      _pathCache = pathCache;
    }

    public void ResolveAllProjectsReferences()
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

    public void Check(IPathRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      ruleSet.Check(_pathCache, analysisReportInProgress);
    }

    public void Check(IProjectScopedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      ruleSet.Check(_projectsById.Values.ToList(), analysisReportInProgress);
    }

    public void Check(INamespacesBasedRuleSet namespacesBasedRuleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      //bug throw new System.NotImplementedException();
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