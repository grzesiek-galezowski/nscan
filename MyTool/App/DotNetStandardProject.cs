using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTool.App
{
  public struct ProjectId
  {
    private string _absolutePath;

    public ProjectId(string absolutePath)
    {
      this._absolutePath = absolutePath;
    }
  }

  public class DotNetStandardProject : IReferencedProject, IReferencingProject
  {
    private readonly Dictionary<ProjectId, IReferencedProject> _referencedProjects = new Dictionary<ProjectId, IReferencedProject>();
    private readonly Dictionary<ProjectId, IReferencingProject> _referencingProjects = new Dictionary<ProjectId, IReferencingProject>();
    private readonly string _assemblyName;
    private readonly ProjectId[] _referencedProjectsIds;


    public DotNetStandardProject(string assemblyName, ProjectId id, ProjectId[] referencedProjectsIds)
    {
      _assemblyName = assemblyName;
      Id = id;
      _referencedProjectsIds = referencedProjectsIds;
    }

    public ProjectId Id { get; }

    private ProjectId[] ReferencedProjectsIds()
    {
      return _referencedProjectsIds;
    }

    public void AddReferencedProject(ProjectId referenceId, IReferencedProject csProject)
    {
      _referencedProjects.Add(referenceId, csProject);
    }


    public void AddReferencingProject(ProjectId parentId, IReferencingProject referencingCsProject)
    {
      AssertThisIsAddingTheSameReferenceNotShadowing(parentId, referencingCsProject);
      _referencingProjects[parentId] = referencingCsProject;
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
      foreach (var referencePath in ReferencedProjectsIds())
      {
        solution.ResolveReferenceFor(this,  referencePath);
      }
    }

    private void AssertThisIsAddingTheSameReferenceNotShadowing(ProjectId referencingProjectPath,
      IReferencingProject referencingCsProject)
    {
      if (_referencingProjects.ContainsKey(referencingProjectPath))
      {
        if (!_referencingProjects[referencingProjectPath].Equals(referencingCsProject))
        {
          throw new Exception("Two distinct projects attempted to be added with the same path");
        }
      }
    }

  }

  public interface IReferencingProject
  {
  }

  public interface IReferencedProject
  {
    void Print(int nestingLevel);
  }
}