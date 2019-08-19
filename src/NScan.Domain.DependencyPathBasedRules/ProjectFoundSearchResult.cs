using System.Collections.Generic;
using System.Linq;

namespace NScan.Domain.DependencyPathBasedRules
{
  public class ProjectFoundSearchResult : IProjectSearchResult
  {
    private readonly IDependencyPathBasedRuleTarget _foundProject;
    private readonly int _occurenceIndex;

    public ProjectFoundSearchResult(IDependencyPathBasedRuleTarget foundProject, int occurenceIndex)
    {
      _foundProject = foundProject;
      _occurenceIndex = occurenceIndex;
    }

    public bool Exists() => true;

    public IReadOnlyList<IDependencyPathBasedRuleTarget> SegmentEndingWith(IProjectSearchResult terminator, IEnumerable<IDependencyPathBasedRuleTarget> path)
    {
      return terminator.TerminatedSegmentStartingFrom(_occurenceIndex, path);
    }

    public IReadOnlyList<IDependencyPathBasedRuleTarget> TerminatedSegmentStartingFrom(int index, IEnumerable<IDependencyPathBasedRuleTarget> path)
    {
      return path.ToList().GetRange(index, _occurenceIndex - index + 1);
    }

    public bool  IsNot(IDependencyPathBasedRuleTarget e)
    {
      return !_foundProject.Equals(e);
    }

    public bool IsNotBefore(IProjectSearchResult dependingProjectSearchResult)
    {
      return dependingProjectSearchResult.IsNotAfter(_occurenceIndex);
    }

    public bool IsNotAfter(int occurenceIndex)
    {
      return occurenceIndex >= _occurenceIndex;
    }
  }
}