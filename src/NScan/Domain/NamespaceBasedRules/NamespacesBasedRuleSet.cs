using System.Collections.Generic;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.NamespaceBasedRules
{
  public class NamespacesBasedRuleSet : INamespacesBasedRuleSet
  {
    private readonly List<INamespacesBasedRule> _rules = new List<INamespacesBasedRule>();

    public void Add(INamespacesBasedRule rule)
    {
      _rules.Add(rule);
    }

    public void Check(IReadOnlyList<INamespaceBasedRuleTarget> dotNetProjects /* bug upper-level interface */, IAnalysisReportInProgress report)
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