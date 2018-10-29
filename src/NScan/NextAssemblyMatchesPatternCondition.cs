using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IDependencyCondition
  {
    bool Matches(IReferencedProject dependency, IProjectSearchResult depending);
    string Description(Glob.Glob dependingAssemblyNamePattern);
  }

  public class NextAssemblyMatchesPatternCondition : IDependencyCondition
  {
    private readonly Glob.Glob _dependencyAssemblyNamePattern;

    public NextAssemblyMatchesPatternCondition(Glob.Glob dependencyAssemblyNamePattern)
    {
      _dependencyAssemblyNamePattern = dependencyAssemblyNamePattern;
    }

    public bool Matches(IReferencedProject dependency, IProjectSearchResult depending)
    {
      return depending.IsNot(dependency)
             && dependency.HasAssemblyNameMatching(_dependencyAssemblyNamePattern);
    }

    public string Description(Glob.Glob dependingAssemblyNamePattern)
    {
      return DependencyDescriptions.IndependentOf(dependingAssemblyNamePattern.Pattern, _dependencyAssemblyNamePattern.Pattern);
    }
  }
}