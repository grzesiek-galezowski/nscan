using GlobExpressions;

namespace TddXt.NScan.Domain
{
  public class HasAssemblyReferenceMatchingCondition : IDependencyCondition
  {
    private readonly Glob _pattern;

    public HasAssemblyReferenceMatchingCondition(Glob pattern)
    {
      _pattern = pattern;
    }

    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return dependency.HasAssemblyReferenceWithNameMatching(_pattern);
    }
  }
}