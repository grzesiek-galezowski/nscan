using GlobExpressions;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public class FollowingAssemblyMatchesPatternCondition : IDependencyCondition
  {
    private readonly Glob _dependencyAssemblyNamePattern;

    public FollowingAssemblyMatchesPatternCondition(Glob dependencyAssemblyNamePattern)
    {
      _dependencyAssemblyNamePattern = dependencyAssemblyNamePattern;
    }

    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return dependency.HasAssemblyNameMatching(_dependencyAssemblyNamePattern);
    }

  }
}