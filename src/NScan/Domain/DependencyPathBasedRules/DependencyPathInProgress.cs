using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public delegate IProjectDependencyPath ProjectDependencyPathFactory(IReadOnlyList<IDependencyPathBasedRuleTarget> projects);

  public class DependencyPathInProgress : IDependencyPathInProgress
  {
    private readonly IFinalDependencyPathDestination _destination;
    private readonly IReadOnlyList<IDependencyPathBasedRuleTarget> _referencedProjects;
    private readonly ProjectDependencyPathFactory _pathFactory;

    public DependencyPathInProgress(
      IFinalDependencyPathDestination destination,
      ProjectDependencyPathFactory pathFactory,
      IReadOnlyList<IDependencyPathBasedRuleTarget> referencedProjects)
    {
      _destination = destination;
      _referencedProjects = referencedProjects;
      _pathFactory = pathFactory;
    }

    public IDependencyPathInProgress CloneWith(IDependencyPathBasedRuleTarget project)
    {
      return new DependencyPathInProgress(
        _destination,
        _pathFactory,
        _referencedProjects.Concat(new [] {project}).ToList()
        );
    }

    public void FinalizeWith(IDependencyPathBasedRuleTarget finalProject)
    {
      IReadOnlyList<IDependencyPathBasedRuleTarget> finalPath = _referencedProjects.Concat(new [] { finalProject}).ToList();
      _destination.Add(_pathFactory(finalPath));
    }
  }
}