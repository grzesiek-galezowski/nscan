using TddXt.NScan.App;

namespace TddXt.NScan.CompositionRoot
{
  public class HasPackageReferenceMatchingCondition : IDependencyCondition
  {
    private readonly Glob.Glob _packagePattern;

    public HasPackageReferenceMatchingCondition(Glob.Glob packagePattern)
    {
      _packagePattern = packagePattern;
    }

    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return dependency.HasPackageReferenceMatching(_packagePattern);
    }
  }
}