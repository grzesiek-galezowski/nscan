using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface IAnalysisReportInProgress
  {
    string AsString();
    void PathViolation(string ruleDescription, IReadOnlyList<IReferencedProject> violationPath);
    void FinishedChecking(string ruleDescription);
    void ProjectScopedViolation(string ruleDescription, string violationDescription);
    bool HasViolations();
  }
}