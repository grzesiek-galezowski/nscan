using GlobExpressions;

namespace NScan.Domain.DependencyPathBasedRules
{
  public class HasAssemblyReferenceMatchingCondition : IDependencyCondition
  {
    private readonly Glob _pattern;

    public HasAssemblyReferenceMatchingCondition(Glob pattern)
    {
      _pattern = pattern;
    }

    public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
    {
      return dependency.HasAssemblyReferenceWithNameMatching(_pattern);
    }
  }
}