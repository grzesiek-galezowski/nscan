using LanguageExt;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules;

public interface IProjectAnalysis
{
  void Perform(IAnalysisReportInProgress analysisReportInProgress);
  void Add(Seq<ProjectScopedRuleUnionDto> rules);
}

public class ProjectAnalysis(
  ISolutionForProjectScopedRules solution,
  IProjectScopedRuleSet projectScopedRuleSet,
  IProjectScopedRuleFactory projectScopedRuleFactory)
  : IProjectAnalysis
{
  public void Perform(IAnalysisReportInProgress analysisReportInProgress)
  {
    solution.Check(projectScopedRuleSet, analysisReportInProgress);
  }

  public void Add(Seq<ProjectScopedRuleUnionDto> rules)
  {
    foreach (var ruleUnionDto in rules)
    {
      ruleUnionDto.Accept(new CreateProjectScopedRuleVisitor(projectScopedRuleFactory, projectScopedRuleSet));
    }
  }

  public static ProjectAnalysis PrepareFor(Seq<CsharpProjectDto> csharpProjectDtos)
  {
    return new ProjectAnalysis(
      new SolutionForProjectScopedRules(
        new ProjectScopedRuleTargetFactory(
          new ProjectScopedRuleViolationFactory()).ProjectScopedRuleTargets(csharpProjectDtos)),
      new ProjectScopedRuleSet(),
      new ProjectScopedRuleFactory(
        new ProjectScopedRuleViolationFactory()));
  }
}
