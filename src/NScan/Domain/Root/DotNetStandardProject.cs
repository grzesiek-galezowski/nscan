using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GlobExpressions;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.Root
{
  public class DotNetStandardProject : IDotNetProject
  {
    private readonly Dictionary<ProjectId, IReferencedProject> _referencedProjects 
      = new Dictionary<ProjectId, IReferencedProject>();
    private readonly Dictionary<ProjectId, IDependencyPathBasedRuleTarget> _referencingProjects 
      = new Dictionary<ProjectId, IDependencyPathBasedRuleTarget>();

    private readonly string _assemblyName;
    private readonly ProjectId[] _referencedProjectsIds;
    private readonly IReadOnlyList<PackageReference> _packageReferences;
    private readonly IReadOnlyList<AssemblyReference> _assemblyReferences;
    private readonly IReadOnlyList<ISourceCodeFile> _files;
    private readonly INamespacesDependenciesCache _namespacesDependenciesCache;
    private readonly INScanSupport _support;
    private readonly ProjectId _id;

    public DotNetStandardProject(string assemblyName,
      ProjectId id,
      ProjectId[] referencedProjectsIds,
      IReadOnlyList<PackageReference> packageReferences,
      IReadOnlyList<AssemblyReference> assemblyReferences,
      IReadOnlyList<ISourceCodeFile> files,
      INamespacesDependenciesCache namespacesDependenciesCache,
      INScanSupport support)
    {
      _assemblyName = assemblyName;
      _id = id;
      _referencedProjectsIds = referencedProjectsIds;
      _packageReferences = packageReferences;
      _assemblyReferences = assemblyReferences;
      _files = files;
      _namespacesDependenciesCache = namespacesDependenciesCache;
      _support = support;
    }

    public void AddReferencedProject(ProjectId projectId, IReferencedProject referencedProject)
    {
      _referencedProjects.Add(projectId, referencedProject);
    }


    public void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject)
    {
      AssertThisIsAddingTheSameReferenceNotShadowing(projectId, referencingProject);
      _referencingProjects[projectId] = referencingProject;
    }

    public bool IsRoot()
    {
      return !_referencingProjects.Any();
    }

    public void Print(int nestingLevel)
    {
      Console.WriteLine(nestingLevel + nestingLevel.Spaces() + _assemblyName);
      foreach (var referencedProjectsValue in _referencedProjects.Values)
      {
        referencedProjectsValue.Print(nestingLevel+1);
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

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private void AssertThisIsAddingTheSameReferenceNotShadowing(
      ProjectId referencingProjectId,
      IDependencyPathBasedRuleTarget referencingProject)
    {
      if (_referencingProjects.ContainsKey(referencingProjectId)
          && !_referencingProjects[referencingProjectId].Equals(referencingProject))
      {
        throw new ProjectShadowingException(_referencingProjects[referencingProjectId], referencingProject);
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

    public bool HasProjectAssemblyNameMatching(Pattern pattern) => 
      pattern.IsMatch(_assemblyName);

    public void RefreshNamespacesCache()
    {
      foreach (var sourceCodeFile in _files)
      {
        sourceCodeFile.AddNamespaceMappingTo(_namespacesDependenciesCache);
      }
    }

    public void Evaluate(INamespacesBasedRule rule, IAnalysisReportInProgress report)
    {
      rule.Evaluate(_assemblyName, _namespacesDependenciesCache, report);
    }

    public bool HasProjectAssemblyNameMatching(Glob glob) => glob.IsMatch(_assemblyName);

    public void ResolveAsReferenceOf(IReferencingProject project)
    {
      project.AddReferencedProject(_id, this);
    }

    public bool HasAssemblyReferenceWithNameMatching(Glob pattern)
    {
      return _assemblyReferences.Any(r => pattern.IsMatch(r.Name));
    }

    public override string ToString()
    {
      return _assemblyName;
    }

    public void AnalyzeFiles(IProjectScopedRule rule, IAnalysisReportInProgress report)
    {
      rule.Check(_files, report);
    }

    public bool HasPackageReferenceMatching(Glob packagePattern)
    {
      return _packageReferences.Any(pr => packagePattern.IsMatch(pr.Name));
    }
  }

  public class ProjectShadowingException : Exception
  {
    public ProjectShadowingException(IDependencyPathBasedRuleTarget previousProject, IDependencyPathBasedRuleTarget newProject)
    : base(
      "Two distinct projects are being added with the same path. " +
      $"{previousProject} would be shadowed by {newProject}. " +
      "This typically indicates a programmer error.")
    {
      
    }
  }
}