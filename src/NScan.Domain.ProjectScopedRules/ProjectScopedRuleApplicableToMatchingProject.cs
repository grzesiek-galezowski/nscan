using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class ProjectScopedRuleApplicableToMatchingProject : IProjectScopedRule
{
  private readonly Pattern _projectAssemblyPattern;
  private readonly IProjectScopedRule _innerRule;

  public ProjectScopedRuleApplicableToMatchingProject(
    Pattern projectAssemblyPattern,
    IProjectScopedRule innerRule)
  {
    _projectAssemblyPattern = projectAssemblyPattern;
    _innerRule = innerRule;
  }

  public RuleDescription Description()
  {
    return _innerRule.Description();
  }

  public void Check(
    IProjectScopedRuleTarget project, 
    IAnalysisReportInProgress report)
  {
    if (project.HasProjectAssemblyNameMatching(_projectAssemblyPattern))
    {
      project.AddInfoAboutMatchingPatternTo(report);
      _innerRule.Check(project, report);
    }
  }
}
