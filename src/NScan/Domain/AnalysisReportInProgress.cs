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
          result.Append(string.Join(Environment.NewLine, _violations[ruleDescription]));
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
      InitializeViolationsFor(ruleDescription);
      //TODO when there is a single project say project, not path
      AddViolation(ruleDescription, _projectPathFormat.ApplyTo(violationPath), "PathViolation in path: ");
    }

    public void FinishedChecking(string ruleDescription)
    {
      AddRuleIfNotRegisteredYet(ruleDescription);

    }

    //bug UT
    public void ProjectScopedViolation(string ruleDescription, string violationDescription)
    {
      AddRuleIfNotRegisteredYet(ruleDescription);
      InitializeViolationsFor(ruleDescription);
      AddViolation(ruleDescription, violationDescription, string.Empty);
    }

    public bool HasViolations()
    {
      return _violations.Any();
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

    private void AddViolation(string ruleDescription, string violationDescription, string prefixPhrase)
    {
      _violations[ruleDescription].Add(prefixPhrase + violationDescription);
    }

  }
}