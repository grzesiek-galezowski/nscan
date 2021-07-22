using System.Collections.Generic;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules
{
  public interface IProjectAnalysis
  {
    void Perform(IAnalysisReportInProgress analysisReportInProgress);
    void Add(IEnumerable<ProjectScopedRuleUnionDto> rules);
  }

  public class ProjectAnalysis : IProjectAnalysis
  {
    private readonly ISolutionForProjectScopedRules _solution;
    private readonly IProjectScopedRuleSet _projectScopedRuleSet;
    private readonly IProjectScopedRuleFactory _projectScopedRuleFactory;

    public ProjectAnalysis(ISolutionForProjectScopedRules solution,
      IProjectScopedRuleSet projectScopedRuleSet,
      IProjectScopedRuleFactory projectScopedRuleFactory)
    {
      _solution = solution;
      _projectScopedRuleSet = projectScopedRuleSet;
      _projectScopedRuleFactory = projectScopedRuleFactory;
    }

    public void Perform(IAnalysisReportInProgress analysisReportInProgress)
    {
      _solution.Check(_projectScopedRuleSet, analysisReportInProgress);
    }

    public void Add(IEnumerable<ProjectScopedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(new CreateProjectScopedRuleVisitor(_projectScopedRuleFactory, _projectScopedRuleSet));
      }
    }

    public static ProjectAnalysis PrepareFor(IEnumerable<CsharpProjectDto> csharpProjectDtos)
    {
      return new ProjectAnalysis(
        new SolutionForProjectScopedRules(new ProjectScopedRuleTargetFactory(new ProjectScopedRuleViolationFactory())
          .ProjectScopedRuleTargets(csharpProjectDtos)),
        new ProjectScopedRuleSet(), 
        new ProjectScopedRuleFactory(
          new ProjectScopedRuleViolationFactory()));
    }
  }
}
