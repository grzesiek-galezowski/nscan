using GlobExpressions;

namespace NScan.Domain.DependencyPathBasedRules
{
  public class HasPackageReferenceMatchingCondition : IDependencyCondition
  {
    private readonly Glob _packagePattern;

    public HasPackageReferenceMatchingCondition(Glob packagePattern)
    {
      _packagePattern = packagePattern;
    }

    public bool Matches(IProjectSearchResult depending, IDependencyPathBasedRuleTarget dependency)
    {
      return dependency.HasPackageReferenceMatching(_packagePattern);
    }
  }
}