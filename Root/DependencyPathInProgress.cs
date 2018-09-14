using System.Collections.Generic;
using System.Linq;
using MyTool.App;

namespace MyTool
{
  public class DependencyPathInProgress : IDependencyPathInProgress
  {
    private readonly IFinalDependencyPathDestination _destination;
    private readonly IReadOnlyList<IReferencedProject> _referencedProjects;

    public DependencyPathInProgress(IFinalDependencyPathDestination destination,
      IReadOnlyList<IReferencedProject> referencedProjects)
    {
      _destination = destination;
      _referencedProjects = referencedProjects;
    }

    public IDependencyPathInProgress CloneWith(IReferencedProject project)
    {
      return new DependencyPathInProgress(_destination, _referencedProjects.Concat(new[] { project }).ToList());
    }

    public void FinalizeWith(IReferencedProject finalProject)
    {
      _destination.Add(_referencedProjects.Concat(new [] { finalProject}).ToList());
    }
  }
}