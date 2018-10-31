﻿

using GlobExpressions;

namespace TddXt.NScan.App
{
  public interface IReferencedProject
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IReferencingProject referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
    void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress);
    bool HasAssemblyNameMatching(Glob glob);

    string ToString();
    bool HasPackageReferenceMatching(Glob packagePattern);
  }
}