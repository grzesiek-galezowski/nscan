using System.Collections.Generic;

namespace NScan.DependencyPathBasedRules;

public class ProjectNotFoundSearchResult : IProjectSearchResult
{
  public bool Exists()
  {
    return false;
  }

  public Seq<IDependencyPathBasedRuleTarget> SegmentEndingWith(IProjectSearchResult terminator, Seq<IDependencyPathBasedRuleTarget> path)
  {
    return Seq<IDependencyPathBasedRuleTarget>.Empty;
  }

  public bool IsNot(IDependencyPathBasedRuleTarget e)
  {
    return true;
  }

  public Seq<IDependencyPathBasedRuleTarget> TerminatedSegmentStartingFrom(int index, Seq<IDependencyPathBasedRuleTarget> path)
  {
    return Seq<IDependencyPathBasedRuleTarget>.Empty;
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
