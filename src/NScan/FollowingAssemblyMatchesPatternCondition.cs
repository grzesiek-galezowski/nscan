using TddXt.NScan.App;

namespace TddXt.NScan
{
  public class FollowingAssemblyMatchesPatternCondition : IDependencyCondition
  {
    private readonly Glob.Glob _dependencyAssemblyNamePattern;

    public FollowingAssemblyMatchesPatternCondition(Glob.Glob dependencyAssemblyNamePattern)
    {
      _dependencyAssemblyNamePattern = dependencyAssemblyNamePattern;
    }

    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return dependency.HasAssemblyNameMatching(_dependencyAssemblyNamePattern);
    }

  }
}