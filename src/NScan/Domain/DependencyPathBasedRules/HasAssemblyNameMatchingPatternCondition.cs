using GlobExpressions;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public class HasAssemblyNameMatchingPatternCondition : IDependencyCondition
  {
    private readonly Glob _dependencyAssemblyNamePattern;

    public HasAssemblyNameMatchingPatternCondition(Glob dependencyAssemblyNamePattern)
    {
      _dependencyAssemblyNamePattern = dependencyAssemblyNamePattern;
    }

    public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
    {
      return dependency.HasProjectAssemblyNameMatching(_dependencyAssemblyNamePattern);
    }

  }
}