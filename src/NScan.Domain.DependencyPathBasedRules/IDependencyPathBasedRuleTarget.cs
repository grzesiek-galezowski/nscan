using GlobExpressions;
using NScan.Lib;

namespace NScan.DependencyPathBasedRules
{
  public interface IDependencyPathBasedRuleTarget
  {
    void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress);
    bool HasProjectAssemblyNameMatching(Pattern glob);
    bool HasProjectAssemblyNameMatching(Glob glob);
    bool HasPackageReferenceMatching(Glob packagePattern);
    bool HasAssemblyReferenceWithNameMatching(Glob pattern);
    string ToString();
  }
}