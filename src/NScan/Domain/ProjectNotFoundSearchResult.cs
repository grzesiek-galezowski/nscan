using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public class ProjectNotFoundSearchResult : IProjectSearchResult
  {
    public bool Exists()
    {
      return false;
    }

    public IReadOnlyList<IReferencedProject> SegmentEndingWith(IProjectSearchResult terminator, IEnumerable<IReferencedProject> path)
    {
      return new List<IReferencedProject>();
    }

    public bool IsNot(IReferencedProject e)
    {
      return true;
    }

    public IReadOnlyList<IReferencedProject> TerminatedSegmentStartingFrom(int index, IEnumerable<IReferencedProject> path)
    {
      return new List<IReferencedProject>();
    }

    public bool IsNotBefore(IProjectSearchResult dependingProjectSearchResult)
    {
      return false;
    }

    public bool IsNotAfter(int occurenceIndex)
    {
      return false;
    }
  }
}