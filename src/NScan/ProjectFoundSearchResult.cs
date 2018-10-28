using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public class ProjectFoundSearchResult : IProjectSearchResult
  {
    private readonly IReferencedProject _foundProject;
    private readonly int _occurenceIndex;

    public ProjectFoundSearchResult(IReferencedProject foundProject, int occurenceIndex)
    {
      _foundProject = foundProject;
      _occurenceIndex = occurenceIndex;
    }

    public bool Exists() => true;

    public IReadOnlyList<IReferencedProject> SegmentEndingWith(IProjectSearchResult terminator, IEnumerable<IReferencedProject> path)
    {
      return terminator.TerminatedSegmentStartingFrom(_occurenceIndex, path);
    }

    public IReadOnlyList<IReferencedProject> TerminatedSegmentStartingFrom(int index, IEnumerable<IReferencedProject> path)
    {
      return path.ToList().GetRange(index, _occurenceIndex - index + 1);
    }

    public bool  IsNot(IReferencedProject e)
    {
      return !_foundProject.Equals(e);
    }

    public bool ExistsAfter(IProjectSearchResult dependingProjectSearchResult)
    {
      return dependingProjectSearchResult.IsBefore(this._occurenceIndex);
    }

    public bool IsBefore(int occurenceIndex)
    {
      return occurenceIndex > _occurenceIndex;
    }
  }
}