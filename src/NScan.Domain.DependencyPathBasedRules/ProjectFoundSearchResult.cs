using System.Collections.Generic;
using System.Linq;

namespace NScan.DependencyPathBasedRules;

public class ProjectFoundSearchResult(IDependencyPathBasedRuleTarget foundProject, int occurenceIndex)
  : IProjectSearchResult
{
  public bool Exists() => true;

  public Seq<IDependencyPathBasedRuleTarget> SegmentEndingWith(
    IProjectSearchResult terminator, 
    Seq<IDependencyPathBasedRuleTarget> path)
  {
    return terminator.TerminatedSegmentStartingFrom(occurenceIndex, path);
  }

  public Seq<IDependencyPathBasedRuleTarget> TerminatedSegmentStartingFrom(
    int index, 
    Seq<IDependencyPathBasedRuleTarget> path)
  {
    //bug this ToList should disappear
    return path.ToList().GetRange(index, occurenceIndex - index + 1).ToSeq();
  }

  public bool IsNot(IDependencyPathBasedRuleTarget e)
  {
    return !foundProject.Equals(e);
  }

  public bool IsNotBefore(IProjectSearchResult dependingProjectSearchResult)
  {
    return dependingProjectSearchResult.IsNotAfter(occurenceIndex);
  }

  public bool IsNotAfter(int index)
  {
    return index >= occurenceIndex;
  }
}
