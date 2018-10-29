using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IDependencyCondition
  {
    bool Matches(IProjectSearchResult depending, IReferencedProject dependency);
  }

  public interface IDescribedDependencyCondition : IDependencyCondition
  {
    string Description();
  }

  public class JoinedDescribedCondition : IDescribedDependencyCondition
  {
    private readonly Glob.Glob _dependencyAssemblyNamePattern;
    private readonly Glob.Glob _dependingAssemblyNamePattern;
    private readonly IDependencyCondition _condition1;
    private readonly IDependencyCondition _condition2;

    public JoinedDescribedCondition(
      Glob.Glob dependencyAssemblyNamePattern,
      Glob.Glob dependingAssemblyNamePattern,
      IDependencyCondition condition1,
      IDependencyCondition condition2)
    {
      _dependencyAssemblyNamePattern = dependencyAssemblyNamePattern;
      _dependingAssemblyNamePattern = dependingAssemblyNamePattern;
      _condition1 = condition1;
      _condition2 = condition2;
    }

    public bool Matches(IProjectSearchResult depending, IReferencedProject dependency)
    {
      return _condition1.Matches(depending, dependency)
             && _condition2
               .Matches(depending, dependency);
    }

    public string Description()
    {
      return DependencyDescriptions.IndependentOf(
        _dependingAssemblyNamePattern.Pattern, 
        _dependencyAssemblyNamePattern.Pattern);
    }
  }
}