using System.Collections.Generic;
using System.Linq;

namespace NScan.DependencyPathBasedRules;

public class ProjectFoundSearchResult(IDependencyPathBasedRuleTarget foundProject, int occurenceIndex)
  : IProjectSearchResult
{
  public bool Exists() => true;

  public IReadOnlyList<IDependencyPathBasedRuleTarget> SegmentEndingWith(
    IProjectSearchResult terminator, 
    IEnumerable<IDependencyPathBasedRuleTarget> path)
  {
    return terminator.TerminatedSegmentStartingFrom(occurenceIndex, path);
  }

  public IReadOnlyList<IDependencyPathBasedRuleTarget> TerminatedSegmentStartingFrom(
    int index, 
    IEnumerable<IDependencyPathBasedRuleTarget> path)
  {
    return path.ToList().GetRange(index, occurenceIndex - index + 1);
  }

  public bool  IsNot(IDependencyPathBasedRuleTarget e)
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
