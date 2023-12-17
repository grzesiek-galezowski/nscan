using GlobExpressions;

namespace NScan.DependencyPathBasedRules;

public class HasPackageReferenceMatchingCondition(Glob packagePattern) : IDependencyCondition
{
  public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
  {
    return dependency.HasPackageReferenceMatching(packagePattern);
  }
}
