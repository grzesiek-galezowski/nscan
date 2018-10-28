using System.Collections.Generic;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IAnalysisReportInProgress
  {
    string AsString();
    void Violation(string ruleDescription, IReadOnlyList<IReferencedProject> violationPath);
    void Ok(string ruleDescription);
    bool HasViolations();
  }
}