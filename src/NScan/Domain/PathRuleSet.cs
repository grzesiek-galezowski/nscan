using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public class PathRuleSet : IPathRuleSet
  {
    private readonly IList<IDependencyRule> _rules = new List<IDependencyRule>();

    public void Add(IDependencyRule rule)
    {
      _rules.Add(rule);
    }

    public void Check(IPathCache cache, IAnalysisReportInProgress report)
    {
      foreach (var dependencyRule in _rules)
      {
        cache.Check(dependencyRule, report);
      }
    }
  }
}