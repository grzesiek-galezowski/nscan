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
      if (!_ruleNames.Contains(ruleDescription))
      {
        _ruleNames.Add(ruleDescription);
      }

      if (!_violations.ContainsKey(ruleDescription))
      {
        _violations.Add(ruleDescription, new HashSet<string>());
      }

      //TODO when there is a single project say project, not path
      _violations[ruleDescription].Add($"PathViolation in path: {_projectPathFormat.ApplyTo(violationPath)}");
    }

    public void Ok(string ruleDescription)
    {
      if (!_ruleNames.Contains(ruleDescription))
      {
        _ruleNames.Add(ruleDescription);
      }

    }

    public bool HasViolations()
    {
      return _violations.Any();
    }
  }
}