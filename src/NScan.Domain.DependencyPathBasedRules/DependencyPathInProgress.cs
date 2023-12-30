using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace NScan.DependencyPathBasedRules;

public class DependencyPathInProgress(
  IFinalDependencyPathDestination destination,
  ProjectDependencyPathFactory pathFactory,
  Seq<IDependencyPathBasedRuleTarget> referencedProjects)
  : IDependencyPathInProgress
{
  public IDependencyPathInProgress CloneWith(IDependencyPathBasedRuleTarget project)
  {
    return new DependencyPathInProgress(
      destination,
      pathFactory,
      referencedProjects.Add(project)
    );
  }

  public void FinalizeWith(IDependencyPathBasedRuleTarget finalProject)
  {
    var finalPath = referencedProjects.Add(finalProject);
    destination.Add(pathFactory(finalPath));
  }
}
