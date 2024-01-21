namespace NScan.DependencyPathBasedRules;

public class PathRuleSet : IPathRuleSet
{
  private Seq<IDependencyRule> _rules = Seq<IDependencyRule>.Empty;

  public void Add(IDependencyRule rule)
  {
    _rules = _rules.Add(rule);
  }

  public void Check(IPathCache cache, IAnalysisReportInProgress report)
  {
    foreach (var dependencyRule in _rules)
    {
      cache.Check(dependencyRule, report);
    }
  }
}
