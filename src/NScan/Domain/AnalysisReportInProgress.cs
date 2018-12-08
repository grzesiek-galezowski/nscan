using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TddXt.NScan.Domain
{
  public class AnalysisReportInProgress : IAnalysisReportInProgress
  {
    private readonly IProjectPathFormat _projectPathFormat;
    private readonly Dictionary<string, HashSet<string>> _violations
      = new Dictionary<string, HashSet<string>>();
    private readonly List<string> _ruleNames = new List<string>();

    public AnalysisReportInProgress(IProjectPathFormat projectPathFormat)
    {
      _projectPathFormat = projectPathFormat;
    }

    public string AsString()
    {
      var result = new StringBuilder();
      for (var index = 0; index < _ruleNames.Count; index++)
      {
        var ruleDescription = _ruleNames[index];
        if (_violations.ContainsKey(ruleDescription))
        {
          result.AppendLine(ruleDescription + ": [ERROR]");
          result.Append(String.Join(Environment.NewLine, _violations[ruleDescription]));
        }
        else
        {
          result.Append(ruleDescription + ": [OK]");
        }

        if (index != _ruleNames.Count - 1)
        {
          result.Append(Environment.NewLine);
        }
      }

      return result.ToString();
    }

    public void PathViolation(string ruleDescription, IReadOnlyList<IReferencedProject> violationPath)
    {
      AddRuleIfNotRegisteredYet(ruleDescription);
      AddViolationsFor(ruleDescription);
      //TODO when there is a single project say project, not path
      _violations[ruleDescription].Add($"PathViolation in path: {_projectPathFormat.ApplyTo(violationPath)}");
    }

    private void AddViolationsFor(string ruleDescription)
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

    public void FinishedChecking(string ruleDescription)
    {
      if (!_ruleNames.Contains(ruleDescription))
      {
        _ruleNames.Add(ruleDescription);
      }

    }

    public void ProjectScopedViolation(string ruleDescription, string violationDescription)
    {
      AddRuleIfNotRegisteredYet(ruleDescription);
      AddViolationsFor(ruleDescription);
      //TODO when there is a single project say project, not path
      _violations[ruleDescription].Add(violationDescription);
    }

    public bool HasViolations()
    {
      return _violations.Any();
    }
  }
}