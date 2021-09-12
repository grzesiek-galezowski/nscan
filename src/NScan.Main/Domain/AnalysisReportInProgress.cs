using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NScan.SharedKernel;

namespace TddXt.NScan.Domain
{
  public class AnalysisReportInProgress : IAnalysisReportInProgress
  {
    private readonly Dictionary<RuleDescription, HashSet<string>> _violationsByRule = new();

    public string AsString()
    {
      var resultBuilder = new ResultBuilder();
      foreach (var kvp in _violationsByRule)
      {
        var (ruleDescription, violations) = kvp;
        if (violations.Any())
        {
          resultBuilder.AppendViolations(ruleDescription, violations);
        }
        else
        {
          resultBuilder.AppendOk(ruleDescription);
        }

        if (!kvp.Equals(_violationsByRule.Last()))
        {
          resultBuilder.AppendRuleSeparator();
        }
      }

      return resultBuilder.Text();
    }


    public void StartedCheckingTarget(string assemblyName)
    {
      //bug throw new System.NotImplementedException();
    }

    public void FinishedEvaluatingRule(string ruleDescription)
    {
      AddRuleIfNotRegisteredYet(new RuleDescription(ruleDescription));
    }

    public bool HasViolations()
    {
      return _violationsByRule.Any(v => v.Value.Any());
    }

    public void Add(RuleViolation ruleViolation)
    {
      InitializeForCollecting(ruleViolation.RuleDescription);
      _violationsByRule[ruleViolation.RuleDescription]
        .Add(ruleViolation.PrefixPhrase + ruleViolation.ViolationDescription);
    }

    private void InitializeForCollecting(RuleDescription ruleName)
    {
      AddRuleIfNotRegisteredYet(ruleName);
    }

    private void AddRuleIfNotRegisteredYet(RuleDescription ruleName)
    {
      if (!_violationsByRule.Keys.Contains(ruleName))
      {
        _violationsByRule[ruleName] = new HashSet<string>();
      }
    }
  }
}
