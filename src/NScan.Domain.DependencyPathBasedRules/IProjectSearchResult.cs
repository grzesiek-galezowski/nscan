namespace NScan.DependencyPathBasedRules;

public interface IProjectSearchResult
{
  bool Exists();
  Seq<IDependencyPathBasedRuleTarget> SegmentEndingWith(IProjectSearchResult terminator, Seq<IDependencyPathBasedRuleTarget> path);
  bool IsNot(IDependencyPathBasedRuleTarget e);
  Seq<IDependencyPathBasedRuleTarget> TerminatedSegmentStartingFrom(int index, Seq<IDependencyPathBasedRuleTarget> path);
  bool IsNotBefore(IProjectSearchResult dependingProjectSearchResult);
  bool IsNotAfter(int occurenceIndex);
}
