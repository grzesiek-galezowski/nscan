﻿using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel.NotifyingSupport.Ports;

namespace NScan.DependencyPathBasedRules;

public interface IReferencedProjects
{
  void Add(ProjectId projectId, IReferencedProject referencedProject);
  void Print(int nestingLevel);
  void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress, IDependencyPathBasedRuleTarget owner);
  void ResolveFrom(IReferencingProject referencingProject, ISolutionContext solution);
}

public class ReferencedProjects(
  Seq<ProjectId> referencedProjectsIds,
  INScanSupport support)
  : IReferencedProjects
{
  private readonly IDictionary<ProjectId, IReferencedProject> _referencedProjects 
    = new Dictionary<ProjectId, IReferencedProject>();

  public void Add(ProjectId projectId, IReferencedProject referencedProject)
  {
    _referencedProjects.Add(projectId, referencedProject);
  }

  public void Print(int nestingLevel)
  {
    foreach (var referencedProjectsValue in _referencedProjects.Values)
    {
      referencedProjectsValue.Print(nestingLevel + 1);
    }
  }

  public void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress, IDependencyPathBasedRuleTarget owner)
  {
    if (_referencedProjects.Any())
    {
      foreach (var reference in _referencedProjects.Values)
      {
        reference.FillAllBranchesOf(dependencyPathInProgress.CloneWith(owner));
      }
    }
    else
    {
      dependencyPathInProgress.FinalizeWith(owner);
    }
  }

  public void ResolveFrom(IReferencingProject referencingProject, ISolutionContext solution)
  {
    foreach (var projectId in referencedProjectsIds)
    {
      try
      {
        solution.ResolveReferenceFrom(referencingProject, projectId);
      }
      catch (ReferencedProjectNotFoundInSolutionException e)
      {
        support.Report(e);
      }
    }
  }
}
