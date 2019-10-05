using System.Collections.Generic;

namespace NScan.DependencyPathBasedRules
{
  public class ProjectNotFoundSearchResult : IProjectSearchResult
  {
    public bool Exists()
    {
      return false;
    }

    public IReadOnlyList<IDependencyPathBasedRuleTarget> SegmentEndingWith(IProjectSearchResult terminator, IEnumerable<IDependencyPathBasedRuleTarget> path)
    {
      return new List<IDependencyPathBasedRuleTarget>();
    }

    public bool IsNot(IDependencyPathBasedRuleTarget e)
    {
      return true;
    }

    public IReadOnlyList<IDependencyPathBasedRuleTarget> TerminatedSegmentStartingFrom(int index, IEnumerable<IDependencyPathBasedRuleTarget> path)
    {
      return new List<IDependencyPathBasedRuleTarget>();
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