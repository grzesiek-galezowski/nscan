using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface ISolutionForProjectScopedRules
  {
    void Check(IProjectScopedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
  }
}