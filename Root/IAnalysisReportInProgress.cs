using System.Collections.Generic;
using MyTool.App;

namespace MyTool
{
  public interface IAnalysisReportInProgress
  {
    string AsString();
    void ViolationOf(string ruleDescription, List<IReferencedProject> violationPath);
    void Ok(string ruleDescription);
    bool HasViolations();
  }
}