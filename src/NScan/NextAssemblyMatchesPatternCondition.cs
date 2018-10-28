using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IDependencyCondition
  {
    bool Matches(IReferencedProject dependency, IProjectSearchResult depending);
    string Description(string dependingAssemblyNamePattern);
  }

  public class NextAssemblyMatchesPatternCondition : IDependencyCondition
  {
    private readonly string _dependencyAssemblyNamePattern;

    public NextAssemblyMatchesPatternCondition(string dependencyAssemblyNamePattern)
    {
      _dependencyAssemblyNamePattern = dependencyAssemblyNamePattern;
    }

    public bool Matches(IReferencedProject dependency, IProjectSearchResult depending)
    {
      return depending.IsNot(dependency)
             && dependency.HasAssemblyNameMatching(_dependencyAssemblyNamePattern);
    }

    public string Description(string dependingAssemblyNamePattern)
    {
      return DependencyDescriptions.IndependentOf(dependingAssemblyNamePattern, _dependencyAssemblyNamePattern);
    }
  }
}