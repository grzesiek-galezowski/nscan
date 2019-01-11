using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface IAnalysisReportInProgress
  {
    void PathViolation(string ruleDescription, IReadOnlyList<IReferencedProject> violationPath);
    void ProjectScopedViolation(string ruleDescription, string violationDescription);
    void NamespacesBasedRuleViolation(string ruleDescription, string projectAssemblyName,
      IReadOnlyList<IReadOnlyList<string>> cycles);
    
    void FinishedChecking(string ruleDescription);
    
    string AsString();
    bool HasViolations();
  }
}