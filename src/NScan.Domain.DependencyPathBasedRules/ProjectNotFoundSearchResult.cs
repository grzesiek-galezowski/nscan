﻿using System.Collections.Generic;
using LanguageExt;

namespace NScan.DependencyPathBasedRules;

public class ProjectNotFoundSearchResult : IProjectSearchResult
{
  public bool Exists()
  {
    return false;
  }

  public Seq<IDependencyPathBasedRuleTarget> SegmentEndingWith(IProjectSearchResult terminator, IEnumerable<IDependencyPathBasedRuleTarget> path)
  {
    return Seq<IDependencyPathBasedRuleTarget>.Empty;
  }

  public bool IsNot(IDependencyPathBasedRuleTarget e)
  {
    return true;
  }

  public Seq<IDependencyPathBasedRuleTarget> TerminatedSegmentStartingFrom(int index, IEnumerable<IDependencyPathBasedRuleTarget> path)
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
