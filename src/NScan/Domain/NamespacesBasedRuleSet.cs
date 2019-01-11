using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public class NamespacesBasedRuleSet : INamespacesBasedRuleSet
  {
    private readonly List<INamespacesBasedRule> _rules = new List<INamespacesBasedRule>();

    public void Add(INamespacesBasedRule rule)
    {
      _rules.Add(rule);
    }

    public void Check(IReadOnlyList<IDotNetProject> dotNetProjects /* bug upper-level interface */, IAnalysisReportInProgress report)
    {
      foreach (var rule in _rules)
      {
        foreach (var dotNetProject in dotNetProjects)
        {
          dotNetProject.Evaluate(rule, report);
        }
        report.FinishedChecking(rule.Description());
      }
    }
  }
}