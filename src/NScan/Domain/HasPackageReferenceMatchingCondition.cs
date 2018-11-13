using GlobExpressions;

namespace TddXt.NScan.Domain
{
  public class HasPackageReferenceMatchingCondition : IDependencyCondition
  {
    private readonly Glob _packagePattern;

    public HasPackageReferenceMatchingCondition(Glob packagePattern)
    {
      _packagePattern = packagePattern;
    }

    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return dependency.HasPackageReferenceMatching(_packagePattern);
    }
  }
}