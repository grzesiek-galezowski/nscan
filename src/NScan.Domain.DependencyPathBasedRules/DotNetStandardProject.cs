using System;
using System.Collections.Generic;
using System.Linq;
using GlobExpressions;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.DependencyPathBasedRules;

public class DotNetStandardProject : IDotNetProject
{
  private readonly string _assemblyName;
  private readonly IReadOnlyList<AssemblyReference> _assemblyReferences;
  private readonly ProjectId _id;
  private readonly IReadOnlyList<PackageReference> _packageReferences;
  private readonly IReferencedProjects _referencedProjects;
  private readonly IReferencingProjects _referencingProjects;

  public DotNetStandardProject(
    string assemblyName,
    ProjectId id,
    IReadOnlyList<PackageReference> packageReferences,
    IReadOnlyList<AssemblyReference> assemblyReferences,
    IReferencedProjects referencedProjects,
    IReferencingProjects referencingProjects)
  {
    _assemblyName = assemblyName;
    _id = id;
    _packageReferences = packageReferences;
    _assemblyReferences = assemblyReferences;
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
    pattern.IsMatchedBy(_assemblyName);

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

  public bool HasPackageReferenceMatching(Glob packagePattern)
  {
    return _packageReferences.Any(pr => packagePattern.IsMatch(pr.Name));
  }
}