using System.Collections.Generic;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IProjectSearchResult
  {
    bool Exists();
    IReadOnlyList<IReferencedProject> SegmentEndingWith(IProjectSearchResult terminator, IEnumerable<IReferencedProject> path);
    bool IsNot(IReferencedProject e);
    IReadOnlyList<IReferencedProject> TerminatedSegmentStartingFrom(int index, IEnumerable<IReferencedProject> path);
    bool ExistsAfter(IProjectSearchResult dependingProjectSearchResult);
    bool IsBefore(int occurenceIndex);
  }
}