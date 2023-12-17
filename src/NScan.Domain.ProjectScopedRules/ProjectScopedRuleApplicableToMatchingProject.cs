using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class ProjectScopedRuleApplicableToMatchingProject(
  Pattern projectAssemblyPattern,
  IProjectScopedRule innerRule)
  : IProjectScopedRule
{
  public RuleDescription Description()
  {
    return innerRule.Description();
  }

  public void Check(
    IProjectScopedRuleTarget project, 
    IAnalysisReportInProgress report)
  {
    if (project.HasProjectAssemblyNameMatching(projectAssemblyPattern))
    {
      project.AddInfoAboutMatchingPatternTo(report);
      innerRule.Check(project, report);
    }
  }
}
