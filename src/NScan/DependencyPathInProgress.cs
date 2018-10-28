using System;
using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public delegate IProjectDependencyPath IProjectDependencyPathFactory(IReadOnlyList<IReferencedProject> projects);

  public class DependencyPathInProgress : IDependencyPathInProgress
  {
    private readonly IFinalDependencyPathDestination _destination;
    private readonly IReadOnlyList<IReferencedProject> _referencedProjects;
    private readonly IProjectDependencyPathFactory _pathFactory;

    public DependencyPathInProgress(
      IFinalDependencyPathDestination destination,
      IProjectDependencyPathFactory pathFactory)
    {
      _destination = destination;
      _referencedProjects = new List<IReferencedProject>();
      _pathFactory = pathFactory;
    }

    public IDependencyPathInProgress CloneWith(IReferencedProject project)
    {
      return new DependencyPathInProgress(_destination, _pathFactory);
    }

    public void FinalizeWith(IReferencedProject finalProject)
    {
      IReadOnlyList<IReferencedProject> finalPath = _referencedProjects.Concat(new [] { finalProject}).ToList();
      _destination.Add(_pathFactory(finalPath));
    }
  }
}