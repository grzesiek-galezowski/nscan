using System.Collections.Generic;
using System.Linq;

namespace TddXt.NScan.Domain
{
  public delegate IProjectDependencyPath ProjectDependencyPathFactory(IReadOnlyList<IReferencedProject> projects);

  public class DependencyPathInProgress : IDependencyPathInProgress
  {
    private readonly IFinalDependencyPathDestination _destination;
    private readonly IReadOnlyList<IReferencedProject> _referencedProjects;
    private readonly ProjectDependencyPathFactory _pathFactory;

    public DependencyPathInProgress(
      IFinalDependencyPathDestination destination,
      ProjectDependencyPathFactory pathFactory,
      IReadOnlyList<IReferencedProject> referencedProjects)
    {
      _destination = destination;
      _referencedProjects = referencedProjects;
      _pathFactory = pathFactory;
    }

    public IDependencyPathInProgress CloneWith(IReferencedProject project)
    {
      return new DependencyPathInProgress(
        _destination,
        _pathFactory,
        _referencedProjects.Concat(new [] {project}).ToList()
        );
    }

    public void FinalizeWith(IReferencedProject finalProject)
    {
      IReadOnlyList<IReferencedProject> finalPath = _referencedProjects.Concat(new [] { finalProject}).ToList();
      _destination.Add(_pathFactory(finalPath));
    }
  }
}