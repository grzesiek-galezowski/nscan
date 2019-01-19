using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Environment;

namespace TddXt.NScan.Domain
{
  public class AnalysisReportInProgress : IAnalysisReportInProgress
  {
    private readonly Dictionary<string, HashSet<string>> _violations
      = new Dictionary<string, HashSet<string>>();
    private readonly List<string> _ruleNames = new List<string>();

    public string AsString()
    {
      var result = new StringBuilder();
      for (var index = 0; index < _ruleNames.Count; index++)
      {
        var ruleDescription = _ruleNames[index];
        if (_violations.ContainsKey(ruleDescription))
        {
          result.AppendLine(ruleDescription + ": [ERROR]");
          result.Append(string.Join(NewLine, _violations[ruleDescription]));
        }
        else
        {
          result.Append(ruleDescription + ": [OK]");
        }

        if (index != _ruleNames.Count - 1)
        {
          result.Append(NewLine);
        }
      }

      return result.ToString();
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