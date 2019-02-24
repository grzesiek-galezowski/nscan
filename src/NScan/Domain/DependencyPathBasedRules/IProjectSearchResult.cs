using System.Collections.Generic;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IProjectSearchResult
  {
    bool Exists();
    IReadOnlyList<IDependencyPathBasedRuleTarget> SegmentEndingWith(IProjectSearchResult terminator, IEnumerable<IDependencyPathBasedRuleTarget> path);
    bool IsNot(IDependencyPathBasedRuleTarget e);
    IReadOnlyList<IDependencyPathBasedRuleTarget> TerminatedSegmentStartingFrom(int index, IEnumerable<IDependencyPathBasedRuleTarget> path);
    bool IsNotBefore(IProjectSearchResult dependingProjectSearchResult);
    bool IsNotAfter(int occurenceIndex);
  }
}