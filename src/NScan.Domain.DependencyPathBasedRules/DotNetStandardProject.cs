using System;
using System.Collections.Generic;
using System.Linq;
using GlobExpressions;
using LanguageExt;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.DependencyPathBasedRules;

public class DotNetStandardProject(
  string assemblyName,
  ProjectId id,
  Arr<PackageReference> packageReferences,
  Arr<AssemblyReference> assemblyReferences,
  IReferencedProjects referencedProjects,
  IReferencingProjects referencingProjects)
  : IDotNetProject
{
  public void AddReferencedProject(ProjectId projectId, IReferencedProject referencedProject)
  {
    referencedProjects.Add(projectId, referencedProject);
  }

  public void AddReferencingProject(ProjectId projectId, IDependencyPathBasedRuleTarget referencingProject)
  {
    referencingProjects.Put(projectId, referencingProject);
  }

  public bool IsRoot()
  {
    return referencingProjects.AreEmpty();
  }

  public void Print(int nestingLevel)
  {
    Console.WriteLine(nestingLevel + nestingLevel.Spaces() + assemblyName);
    referencedProjects.Print(nestingLevel);
  }

  public void ResolveReferencesFrom(ISolutionContext solution)
  {
    referencedProjects.ResolveFrom(this, solution);
  }

  public void ResolveAsReferencing(IReferencedProject project)
  {
    project.AddReferencingProject(id, this);
  }

  public void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress)
  {
    referencedProjects.FillAllBranchesOf(dependencyPathInProgress, this);
  }

  public bool HasProjectAssemblyNameMatching(Pattern pattern) => 
    pattern.IsMatchedBy(assemblyName);

  public bool HasProjectAssemblyNameMatching(Glob glob) => glob.IsMatch(assemblyName);

  public void ResolveAsReferenceOf(IReferencingProject project)
  {
    project.AddReferencedProject(id, this);
  }

  public bool HasAssemblyReferenceWithNameMatching(Glob pattern)
  {
    return assemblyReferences.Any(r => pattern.IsMatch(r.Name));
  }

  public override string ToString()
  {
    return assemblyName;
  }

  public bool HasPackageReferenceMatching(Glob packagePattern)
  {
    return packageReferences.Any(pr => packagePattern.IsMatch(pr.Name));
  }
}
