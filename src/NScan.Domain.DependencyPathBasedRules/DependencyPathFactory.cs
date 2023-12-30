using LanguageExt;

namespace NScan.DependencyPathBasedRules;

public class DependencyPathFactory : IDependencyPathFactory
{
  public IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination)
  {
    return new DependencyPathInProgress(
      destination,
      projects => new ProjectDependencyPath(
        projects,
        new ProjectFoundSearchResultFactory()),
      Seq<IDependencyPathBasedRuleTarget>.Empty);
  }
}
