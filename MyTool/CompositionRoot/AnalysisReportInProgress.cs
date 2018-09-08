using System.Collections.Generic;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class AnalysisReportInProgress : IAnalysisReportInProgress
  {
    public string AsString()
    {
      throw new System.NotImplementedException();
    }

    public void ViolationOf(string ruleDescription, List<IReferencedProject> violationPath)
    {
      throw new System.NotImplementedException();
    }
  }
}