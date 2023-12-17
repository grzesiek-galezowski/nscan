using GlobExpressions;

namespace NScan.DependencyPathBasedRules;

public class HasAssemblyReferenceMatchingCondition(Glob pattern) : IDependencyCondition
{
  public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
  {
    return dependency.HasAssemblyReferenceWithNameMatching(pattern);
  }
}
