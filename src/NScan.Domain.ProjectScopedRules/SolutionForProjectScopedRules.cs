using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public interface ISolutionForProjectScopedRules
{
  void Check(IProjectScopedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
}

public class SolutionForProjectScopedRules(IReadOnlyList<IProjectScopedRuleTarget> projectScopedRuleTargets)
  : ISolutionForProjectScopedRules
{
  public void Check(IProjectScopedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
  {
    ruleSet.Check(projectScopedRuleTargets, analysisReportInProgress);
  }
}
