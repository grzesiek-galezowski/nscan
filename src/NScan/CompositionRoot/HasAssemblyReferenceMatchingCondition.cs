using GlobExpressions;
using TddXt.NScan.App;

namespace TddXt.NScan.CompositionRoot
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