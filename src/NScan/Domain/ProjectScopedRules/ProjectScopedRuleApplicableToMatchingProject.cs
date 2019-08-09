using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
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

    public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
    {
      if (project.HasProjectAssemblyNameMatching(_projectAssemblyPattern))
      {
        _innerRule.Check(project, report);
      }
    }

    public override string ToString()
    {
      return _innerRule.ToString();
    }
  }
}