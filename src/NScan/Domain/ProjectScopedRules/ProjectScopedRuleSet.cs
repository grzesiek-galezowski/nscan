using System.Collections.Generic;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public class ProjectScopedRuleSet : IProjectScopedRuleSet
  {
    private readonly List<IProjectScopedRule> _rules = new List<IProjectScopedRule>();

    public void Add(IProjectScopedRule rule)
    {
      _rules.Add(rule);
    }

    public void Check(IReadOnlyList<IProjectScopedRuleTarget> dotNetProjects, IAnalysisReportInProgress report)
    {
      foreach (var target in dotNetProjects)
      {
        foreach (var rule in _rules)
        {
          rule.Check(target, report);
          report.FinishedChecking(rule.ToString());
        }
      }
    }
  }
}