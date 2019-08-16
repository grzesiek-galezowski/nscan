using System.Collections.Generic;

namespace NScan.Domain.DependencyPathBasedRules
{
  public class DependencyPathFactory : IDependencyPathFactory
  {
    public IDependencyPathInProgress NewDependencyPathFor(IFinalDependencyPathDestination destination)
    {
      return new DependencyPathInProgress(
        destination, 
        projects => new ProjectDependencyPath(projects, new ProjectFoundSearchResultFactory()), new List<IDependencyPathBasedRuleTarget>());
    }
  }
}