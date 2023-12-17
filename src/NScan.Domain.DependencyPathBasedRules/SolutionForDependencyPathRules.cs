﻿using System;
using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules;

public class SolutionForDependencyPathRules(
  IPathCache pathCache,
  IReadOnlyDictionary<ProjectId, IDotNetProject> projectsById)
  : ISolutionForDependencyPathBasedRules, ISolutionContext
{
  public void ResolveAllProjectsReferences()
  {
    //backlog use the analysis report to write what projects are skipped - write a separate acceptance test for that
    foreach (var referencingProject in projectsById.Values)
    {
      referencingProject.ResolveReferencesFrom(this);
    }
  }

  public void PrintDebugInfo()
  {
    foreach (var project in projectsById.Values.Where(v => v.IsRoot()))
    {
      project.Print(0);
    }
  }

  public void ResolveReferenceFrom(IReferencingProject referencingProject, ProjectId referencedProjectId)
  {
    try
    {
      var referencedProject = projectsById[referencedProjectId];

      referencingProject.ResolveAsReferencing(referencedProject);
      referencedProject.ResolveAsReferenceOf(referencingProject);
    }
    catch (KeyNotFoundException e)
    {
      throw new ReferencedProjectNotFoundInSolutionException(
        CouldNotFindProjectFor(referencedProjectId, projectsById), e);
    }
  }

  private static string CouldNotFindProjectFor(ProjectId referencedProjectId,
    IReadOnlyDictionary<ProjectId, IDotNetProject> projectsById)
  {
    const string dotString = "* ";
    return
      $"Could not find referenced project {referencedProjectId} " +
      "probably because it was in an incompatible format " +
      "and was skipped during project collection phase. " +
      "Existing project keys: " +
      dotString + $"{string.Join(Environment.NewLine + dotString, projectsById.Keys)}";
  }

  public void Check(IPathRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
  {
    ruleSet.Check(pathCache, analysisReportInProgress);
  }

  public void BuildDependencyPathCache()
  {
    pathCache.BuildStartingFrom(RootProjects());
  }

  private IDotNetProject[] RootProjects()
  {
    return Projects().Where(project => project.IsRoot()).ToArray();
  }

  private IReadOnlyList<IDotNetProject> Projects()
  {
    return projectsById.Values.ToList();
  }

}
