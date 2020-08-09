using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface ISolutionForProjectScopedRules
  {
    void Check(IProjectScopedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
  }

  public class SolutionForProjectScopedRules : ISolutionForProjectScopedRules //bug move
  {
    private readonly IReadOnlyList<IProjectScopedRuleTarget> _projectScopedRuleTargets;

    public SolutionForProjectScopedRules(IReadOnlyList<IProjectScopedRuleTarget> projectScopedRuleTargets)
    {
      _projectScopedRuleTargets = projectScopedRuleTargets;
    }

    public void Check(IProjectScopedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      ruleSet.Check(_projectScopedRuleTargets, analysisReportInProgress);
    }
  }
}