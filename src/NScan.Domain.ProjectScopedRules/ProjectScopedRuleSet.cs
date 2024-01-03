using System.Collections.Generic;
using LanguageExt;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class ProjectScopedRuleSet : IProjectScopedRuleSet
{
  private readonly List<IProjectScopedRule> _rules = new();

  public void Add(IProjectScopedRule rule)
  {
    _rules.Add(rule);
  }

  public void Check(Seq<IProjectScopedRuleTarget> dotNetProjects, IAnalysisReportInProgress report)
  {
    foreach (var target in dotNetProjects)
    {
      foreach (var rule in _rules)
      {
        rule.Check(target, report);
        report.FinishedEvaluatingRule(rule.Description());
      }
    }
  }
}
