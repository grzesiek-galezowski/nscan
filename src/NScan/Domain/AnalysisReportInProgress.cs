using System.Collections.Generic;
using System.Linq;

namespace TddXt.NScan.Domain
{
  public class AnalysisReportInProgress : IAnalysisReportInProgress
  {
    private readonly Dictionary<string, HashSet<string>> _violations
      = new Dictionary<string, HashSet<string>>();
    private readonly List<string> _ruleNames = new List<string>();

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


    public void FinishedChecking(string ruleDescription)
    {
      AddRuleIfNotRegisteredYet(ruleDescription);
    }


    public bool HasViolations()
    {
      return _violations.Any();
    }

    private void InitializeForCollecting(string ruleDescription)
    {
      AddRuleIfNotRegisteredYet(ruleDescription);
      InitializeViolationsFor(ruleDescription);
    }

    private void InitializeViolationsFor(string ruleDescription)
    {
      if (!_violations.ContainsKey(ruleDescription))
      {
        _violations.Add(ruleDescription, new HashSet<string>());
      }
    }

    private void AddRuleIfNotRegisteredYet(string ruleDescription)
    {
      if (!_ruleNames.Contains(ruleDescription))
      {
        _ruleNames.Add(ruleDescription);
      }
    }

    public void Add(RuleViolation ruleViolation)
    {
      InitializeForCollecting(ruleViolation.RuleDescription);
      _violations[ruleViolation.RuleDescription].Add(ruleViolation.PrefixPhrase + ruleViolation.ViolationDescription);
    }

  }
}