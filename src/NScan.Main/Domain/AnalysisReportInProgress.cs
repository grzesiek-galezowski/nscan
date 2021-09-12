using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NScan.SharedKernel;

namespace TddXt.NScan.Domain
{
  public class AnalysisReportInProgress : IAnalysisReportInProgress
  {
    private readonly List<RuleDescription> _ruleNames = new();

    private readonly Dictionary<RuleDescription, HashSet<string>> _violations = new();

    public string AsString()
    {
      var resultBuilder = new ResultBuilder();
      for (var index = 0; index < _ruleNames.Count; index++)
      {
        AppendRuleResult(index, resultBuilder);
        AppendNewLineIfNotLastRule(index, resultBuilder);
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
      return _violations.Any();
    }

    public void Add(RuleViolation ruleViolation)
    {
      InitializeForCollecting(ruleViolation.RuleDescription);
      _violations[ruleViolation.RuleDescription]
        .Add(ruleViolation.PrefixPhrase + ruleViolation.ViolationDescription);
    }

    private void AppendRuleResult(int index, ResultBuilder resultBuilder)
    {
      var ruleDescription = _ruleNames[index];
      if (_violations.ContainsKey(ruleDescription))
      {
        resultBuilder.AppendViolations(ruleDescription, _violations);
      }
      else
      {
        resultBuilder.AppendOk(ruleDescription);
      }
    }

    private void AppendNewLineIfNotLastRule(int index, ResultBuilder resultBuilder)
    {
      if (index != _ruleNames.Count - 1)
      {
        resultBuilder.AppendRuleSeparator();
      }
    }

    private void InitializeForCollecting(RuleDescription ruleName)
    {
      AddRuleIfNotRegisteredYet(ruleName);
      InitializeViolationsFor(ruleName);
    }

    private void InitializeViolationsFor(RuleDescription ruleName)
    {
      if (!_violations.ContainsKey(ruleName))
      {
        _violations.Add(ruleName, new HashSet<string>());
      }
    }

    private void AddRuleIfNotRegisteredYet(RuleDescription ruleName)
    {
      if (!_ruleNames.Contains(ruleName))
      {
        _ruleNames.Add(ruleName);
      }
    }
  }
}
