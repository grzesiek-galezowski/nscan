using System.Collections.Generic;
using System.Linq;

namespace NScan.DependencyPathBasedRules;

public delegate IProjectDependencyPath ProjectDependencyPathFactory(IReadOnlyList<IDependencyPathBasedRuleTarget> projects);

public class DependencyPathInProgress(
  IFinalDependencyPathDestination destination,
  ProjectDependencyPathFactory pathFactory,
  IReadOnlyList<IDependencyPathBasedRuleTarget> referencedProjects)
  : IDependencyPathInProgress
{
  public IDependencyPathInProgress CloneWith(IDependencyPathBasedRuleTarget project)
  {
    return new DependencyPathInProgress(
      destination,
      pathFactory,
      referencedProjects.Concat([project]).ToList()
    );
  }

  public void FinalizeWith(IDependencyPathBasedRuleTarget finalProject)
  {
    IReadOnlyList<IDependencyPathBasedRuleTarget> finalPath 
      = referencedProjects.Concat([finalProject]).ToList();
    destination.Add(pathFactory(finalPath));
  }
}
