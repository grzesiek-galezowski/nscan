using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface IProjectSearchResult
  {
    bool Exists();
    IReadOnlyList<IReferencedProject> SegmentEndingWith(IProjectSearchResult terminator, IEnumerable<IReferencedProject> path);
    bool IsNot(IReferencedProject e);
    IReadOnlyList<IReferencedProject> TerminatedSegmentStartingFrom(int index, IEnumerable<IReferencedProject> path);
    bool IsNotBefore(IProjectSearchResult dependingProjectSearchResult);
    bool IsNotAfter(int occurenceIndex);
  }
}