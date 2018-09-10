using System;
using System.Collections.Generic;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class AnalysisReportInProgress : IAnalysisReportInProgress
  {
    private readonly List<string> _messages = new List<string>();

    public string AsString()
    {
      return string.Join(Environment.NewLine, _messages);
    }

    public void ViolationOf(string ruleDescription, List<IReferencedProject> violationPath)
    {
      throw new System.NotImplementedException();
    }

    public void Ok(string ruleDescription)
    {
      _messages.Add(ruleDescription + ": [OK]");
    }

    //todo no duplicate paths!!!
  }
}