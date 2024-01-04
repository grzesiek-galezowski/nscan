using LanguageExt;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public class NamespacesBasedRuleSet : INamespacesBasedRuleSet
{
  private Seq<INamespacesBasedRule> _rules;

  public void Add(INamespacesBasedRule rule)
  {
    _rules = _rules.Add(rule);
  }

  public void Check(Seq<INamespaceBasedRuleTarget> dotNetProjects, IAnalysisReportInProgress report)
  {
    foreach (var rule in _rules)
    {
      foreach (var dotNetProject in dotNetProjects)
      {
        dotNetProject.Evaluate(rule, report);
      }
      report.FinishedEvaluatingRule(rule.Description());
    }
  }
}
