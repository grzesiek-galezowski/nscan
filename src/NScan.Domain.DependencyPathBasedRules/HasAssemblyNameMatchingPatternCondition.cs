using GlobExpressions;

namespace NScan.DependencyPathBasedRules;

public class HasAssemblyNameMatchingPatternCondition(Glob dependencyAssemblyNamePattern) : IDependencyCondition
{
  public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
  {
    return dependency.HasProjectAssemblyNameMatching(dependencyAssemblyNamePattern);
  }
}
