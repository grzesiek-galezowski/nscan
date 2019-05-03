using System;
using System.Collections.Generic;
using System.Linq;
using GlobExpressions;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.Lib;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.Root
{
  public class DotNetStandardProject : IDotNetProject
  {
    private readonly string _assemblyName;
    private readonly IReadOnlyList<AssemblyReference> _assemblyReferences;
    private readonly IReadOnlyList<ISourceCodeFile> _files;
    private readonly ProjectId _id;
    private readonly INamespacesDependenciesCache _namespacesDependenciesCache;
    private readonly IReadOnlyList<PackageReference> _packageReferences;
    private readonly IReferencedProjects _referencedProjects;
    private readonly IReferencingProjects _referencingProjects;

    public DotNetStandardProject(
      string assemblyName,
      ProjectId id,
      IReadOnlyList<PackageReference> packageReferences,
      IReadOnlyList<AssemblyReference> assemblyReferences,
      IReadOnlyList<ISourceCodeFile> files,
      INamespacesDependenciesCache namespacesDependenciesCache, 
      IReferencedProjects referencedProjects, 
      IReferencingProjects referencingProjects)
    {
      _assemblyName = assemblyName;
      _id = id;
      _packageReferences = packageReferences;
      _assemblyReferences = assemblyReferences;
      _files = files;
      _namespacesDependenciesCache = namespacesDependenciesCache;
      _referencedProjects = referencedProjects;
      _referencingProjects = referencingProjects;
    }

    public void AddReferencedProject(ProjectId projectId, IReferencedProject referencedProject)
    {
      _referencedProjects.Add(projectId, referencedProject);
    }

    public void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject)
    {
      _referencingProjects.Put(projectId, referencingProject);
    }

    public bool IsRoot()
    {
      return _referencingProjects.AreEmpty();
    }

    public void Print(int nestingLevel)
    {
      Console.WriteLine(nestingLevel + nestingLevel.Spaces() + _assemblyName);
      _referencedProjects.Print(nestingLevel);
    }

    public void ResolveReferencesFrom(ISolutionContext solution)
    {
      _referencedProjects.ResolveFrom(this, solution);
    }

    public void ResolveAsReferencing(IReferencedProject project)
    {
      project.AddReferencingProject(_id, this);
    }

    public void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress)
    {
      _referencedProjects.FillAllBranchesOf(dependencyPathInProgress, this);
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
}