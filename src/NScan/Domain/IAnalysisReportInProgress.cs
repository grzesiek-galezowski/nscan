using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface IAnalysisReportInProgress
  {
    string AsString();
    void Violation(string ruleDescription, IReadOnlyList<IReferencedProject> violationPath);
    void Ok(string ruleDescription);
    bool HasViolations();
  }
}