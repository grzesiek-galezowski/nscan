

using GlobExpressions;
using TddXt.NScan.CompositionRoot;

namespace TddXt.NScan.App
{
  public interface IReferencedProject
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IReferencingProject referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
    void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress);
    bool HasProjectAssemblyNameMatching(Pattern glob);
    bool HasProjectAssemblyNameMatching(Glob glob);
    bool HasPackageReferenceMatching(Glob packagePattern);
    bool HasAssemblyReferenceWithNameMatching(Glob pattern);

    string ToString();
  }
}