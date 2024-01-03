using System.Collections.Generic;

namespace NScan.DependencyPathBasedRules;

public interface IProjectSearchResult
{
  bool Exists();
  Seq<IDependencyPathBasedRuleTarget> SegmentEndingWith(IProjectSearchResult terminator, IEnumerable<IDependencyPathBasedRuleTarget> path);
  bool IsNot(IDependencyPathBasedRuleTarget e);
  Seq<IDependencyPathBasedRuleTarget> TerminatedSegmentStartingFrom(int index, IEnumerable<IDependencyPathBasedRuleTarget> path);
  bool IsNotBefore(IProjectSearchResult dependingProjectSearchResult);
  bool IsNotAfter(int occurenceIndex);
}
