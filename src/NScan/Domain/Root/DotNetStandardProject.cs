using System;
using System.Collections.Generic;
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
    private readonly string _assemblyName;
    private readonly IReadOnlyList<PackageReference> _packageReferences;
    private readonly IReadOnlyList<AssemblyReference> _assemblyReferences;
    private readonly IReadOnlyList<ISourceCodeFile> _files;
    private readonly INamespacesDependenciesCache _namespacesDependenciesCache;
    private readonly ProjectId _id;

    public DotNetStandardProject(
      string assemblyName,
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
      _packageReferences = packageReferences;
      _assemblyReferences = assemblyReferences;
      _files = files;
      _namespacesDependenciesCache = namespacesDependenciesCache;
      ReferencedProjects = new ReferencedProjects(referencedProjectsIds, support);
      ReferencingProjects = new ReferencingProjects();
    }

    private ReferencedProjects ReferencedProjects { get; }

    private ReferencingProjects ReferencingProjects { get; }

    public void AddReferencedProject(ProjectId projectId, IReferencedProject referencedProject)
    {
      ReferencedProjects.Add(projectId, referencedProject);
    }

    public void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject)
    {
      ReferencingProjects.Put(projectId, referencingProject);
    }

    public bool IsRoot()
    {
      return ReferencingProjects.AreEmpty();
    }

    public void Print(int nestingLevel)
    {
      Console.WriteLine(nestingLevel + nestingLevel.Spaces() + _assemblyName);
      ReferencedProjects.Print(nestingLevel);
    }

    public void ResolveReferencesFrom(ISolutionContext solution)
    {
      this.ReferencedProjects.ResolveFrom(this, solution);
    }

    public void ResolveAsReferencing(IReferencedProject project)
    {
      project.AddReferencingProject(_id, this);
    }

    public void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress)
    {
      ReferencedProjects.FillAllBranchesOf(dependencyPathInProgress, this);
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