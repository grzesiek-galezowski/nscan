using GlobExpressions;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IDependencyPathBasedRuleTarget
  {
    bool HasProjectAssemblyNameMatching(Pattern glob);
    bool HasProjectAssemblyNameMatching(Glob glob);
    bool HasPackageReferenceMatching(Glob packagePattern);
    bool HasAssemblyReferenceWithNameMatching(Glob pattern);
    void FillAllBranchesOf(IDependencyPathInProgress dependencyPathInProgress);
    string ToString();
  }
}