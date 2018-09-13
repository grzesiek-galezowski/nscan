using System.Collections.Generic;
using System.Text;
using MyTool.App;
using static System.Environment;
using static System.String;

namespace MyTool.CompositionRoot
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
          result.Append(Join(NewLine, _violations[ruleDescription]));
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

    public void ViolationOf(string ruleDescription, List<IReferencedProject> violationPath)
    {
      if (!_ruleNames.Contains(ruleDescription))
      {
        _ruleNames.Add(ruleDescription);
      }

      if (!_violations.ContainsKey(ruleDescription))
      {
        _violations.Add(ruleDescription, new HashSet<string>());
      }

      _violations[ruleDescription].Add($"Violation in path: {_projectPathFormat.ApplyTo(violationPath)}");
    }

    public void Ok(string ruleDescription)
    {
      if (!_ruleNames.Contains(ruleDescription))
      {
        _ruleNames.Add(ruleDescription);
      }

    }
  }
}