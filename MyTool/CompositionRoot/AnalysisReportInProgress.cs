using System.Collections.Generic;
using System.Text;
using MyTool.App;
using static System.Environment;

namespace MyTool.CompositionRoot
{
  public class AnalysisReportInProgress : IAnalysisReportInProgress
  {
    private readonly IProjectPathFormat _projectPathFormat;
    private readonly List<string> _okRuleNames = new List<string>();
    private readonly Dictionary<string, List<string>> _violations = new Dictionary<string, List<string>>();
    private List<string> _ruleNames = new List<string>();

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

      return string.Join(NewLine, _messages);
    }

    public void ViolationOf(string ruleDescription, List<IReferencedProject> violationPath)
    {
      if (!_ruleNames.Contains(ruleDescription))
      {
        _ruleNames.Add(ruleDescription);
      }

      if (!_violations.ContainsKey(ruleDescription))
      {
        _violations.Add(ruleDescription, new List<string>());
      }

      _violations[ruleDescription].Add($"{ruleDescription}: [ERROR]{NewLine}Violation in path: {_projectPathFormat.ApplyTo(violationPath)}");
    }

    public void Ok(string ruleDescription)
    {
      if (!_ruleNames.Contains(ruleDescription))
      {
        _ruleNames.Add(ruleDescription);
      }

      _okRuleNames.Add(ruleDescription);
      //_messages[ruleDescription].Add(ruleDescription + ": [OK]");
    }

    //todo no duplicate paths!!!
  }
}