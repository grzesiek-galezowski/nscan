using GlobExpressions;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public class HasAssemblyNameMatchingPatternCondition : IDependencyCondition
  {
    private readonly Glob _dependencyAssemblyNamePattern;

    public HasAssemblyNameMatchingPatternCondition(Glob dependencyAssemblyNamePattern)
    {
      _dependencyAssemblyNamePattern = dependencyAssemblyNamePattern;
    }

    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return dependency.HasProjectAssemblyNameMatching(_dependencyAssemblyNamePattern);
    }

  }
}