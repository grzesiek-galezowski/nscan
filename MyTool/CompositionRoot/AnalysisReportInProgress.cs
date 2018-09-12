using System.Collections.Generic;
using MyTool.App;
using static System.Environment;

namespace MyTool.CompositionRoot
{
  public class AnalysisReportInProgress : IAnalysisReportInProgress
  {
    private readonly IProjectPathFormat _projectPathFormat;
    private readonly List<string> _messages = new List<string>();

    public AnalysisReportInProgress(IProjectPathFormat projectPathFormat)
    {
      _projectPathFormat = projectPathFormat;
    }

    public string AsString()
    {
      return string.Join(NewLine, _messages);
    }

    public void ViolationOf(string ruleDescription, List<IReferencedProject> violationPath)
    {
      _messages.Add($"{ruleDescription}: [ERROR]{NewLine}{_projectPathFormat.ApplyTo(violationPath)}");
    }

    public void Ok(string ruleDescription)
    {
      _messages.Add(ruleDescription + ": [OK]");
    }

    //todo no duplicate paths!!!
  }
}