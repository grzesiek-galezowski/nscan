using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyTool.App
{
  public class DotNetStandardProject : IDotNetProject
  {
    private readonly Dictionary<ProjectId, IReferencedProject> _referencedProjects = new Dictionary<ProjectId, IReferencedProject>();
    private readonly Dictionary<ProjectId, IReferencingProject> _referencingProjects = new Dictionary<ProjectId, IReferencingProject>();
    private readonly string _assemblyName;
    private readonly ProjectId[] _referencedProjectsIds;
    private readonly ISupport _support;
    private readonly ProjectId _id;

    public DotNetStandardProject(string assemblyName, ProjectId id, ProjectId[] referencedProjectsIds, ISupport support)
    {
      _assemblyName = assemblyName;
      _id = id;
      _referencedProjectsIds = referencedProjectsIds;
      _support = support;
    }

    public void AddReferencedProject(ProjectId referencedProjectId, IReferencedProject referencedProject)
    {
      _referencedProjects.Add(referencedProjectId, referencedProject);
    }


    public void AddReferencingProject(ProjectId referencingProjectId, IReferencingProject referencingCsProject)
    {
      AssertThisIsAddingTheSameReferenceNotShadowing(referencingProjectId, referencingCsProject);
      _referencingProjects[referencingProjectId] = referencingCsProject;
    }

    public bool IsRoot()
    {
      return !_referencingProjects.Any();
    }

    public void Print(int i)
    {
      Console.WriteLine(i + i.Spaces() + _assemblyName);
      foreach (var referencedProjectsValue in _referencedProjects.Values)
      {
        referencedProjectsValue.Print(i+1);
      }
    }

    public void ResolveReferencesFrom(ISolutionContext solution)
    {
      foreach (var projectId in _referencedProjectsIds)
      {
        try
        {
          solution.ResolveReferenceFrom(this, projectId);
        }
        catch (ReferencedProjectNotFoundInSolutionException e)
        {
          _support.Report(e);
        }
      }
    }

    private void AssertThisIsAddingTheSameReferenceNotShadowing(
      ProjectId referencingProjectId,
      IReferencingProject referencingProject)
    {
      if (_referencingProjects.ContainsKey(referencingProjectId))
      {
        if (!_referencingProjects[referencingProjectId].Equals(referencingProject))
        {
          throw new Exception("Two distinct projects attempted to be added with the same path");
        }
      }
    }

    public void ResolveAsReferencing(IReferencedProject project)
    {
      project.AddReferencingProject(_id, this);
    }

    public void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress)
    {
      if (_referencedProjects.Any())
      {
        foreach (var reference in _referencedProjects.Values)
        {
          reference.FillAllBranchesOf(dependencyPathInProgress.CloneWith(this));
        }
      }
      else
      {
        dependencyPathInProgress.FinalizeWith(this);
      }

    }

    public bool HasAssemblyName(string assemblyName) => _assemblyName == assemblyName;

    public void ResolveAsReferenceOf(IReferencingProject project)
    {
      project.AddReferencedProject(_id, this);
    }
  }
}