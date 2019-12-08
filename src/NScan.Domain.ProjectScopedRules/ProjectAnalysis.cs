using System.Collections.Generic;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules
{
  public interface IProjectAnalysis
  {
    void PerformOn(ISolutionForProjectScopedRules solution, IAnalysisReportInProgress analysisReportInProgress);
    void Add(IEnumerable<ProjectScopedRuleUnionDto> rules);
  }

  public class ProjectAnalysis : IProjectAnalysis
  {
    private readonly IProjectScopedRuleSet _projectScopedRuleSet;
    private readonly IProjectScopedRuleFactory _projectScopedRuleFactory;

    public ProjectAnalysis(
      IProjectScopedRuleSet projectScopedRuleSet,
      IProjectScopedRuleFactory projectScopedRuleFactory)
    {
      _projectScopedRuleSet = projectScopedRuleSet;
      _projectScopedRuleFactory = projectScopedRuleFactory;
    }

    public void PerformOn(ISolutionForProjectScopedRules solution, IAnalysisReportInProgress analysisReportInProgress)
    {
      solution.Check(_projectScopedRuleSet, analysisReportInProgress);
    }

    public void Add(IEnumerable<ProjectScopedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(new CreateProjectScopedRuleVisitor(_projectScopedRuleFactory, _projectScopedRuleSet));
      }
    }
  }
}