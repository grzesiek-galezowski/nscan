using System.Collections.Generic;
using NScan.Domain.Root;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class ProjectAnalysisSpecification
  {
    [Fact]
    public void ShouldAddRuleToRuleSet()
    {
      //GIVEN
      var ruleSet = Substitute.For<IProjectScopedRuleSet>();
      var ruleFactory = Substitute.For<IProjectScopedRuleFactory>();
      var projectAnalysis = new ProjectAnalysis(ruleSet, ruleFactory);
      var dto1 = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var dto2 = Any.Instance<HasAttributesOnRuleComplementDto>();
      var dto3 = Any.Instance<HasTargetFrameworkRuleComplementDto>();
      var projectScopedRuleUnionDtos = new List<ProjectScopedRuleUnionDto>()
      {
        ProjectScopedRuleUnionDto.With(dto1), 
        ProjectScopedRuleUnionDto.With(dto2), 
        ProjectScopedRuleUnionDto.With(dto3)
      };

      var rule1 = Any.Instance<IProjectScopedRule>();
      var rule2 = Any.Instance<IProjectScopedRule>();
      var rule3 = Any.Instance<IProjectScopedRule>();
      ruleFactory.CreateProjectScopedRuleFrom(dto1).Returns(rule1);
      ruleFactory.CreateProjectScopedRuleFrom(dto2).Returns(rule2);
      ruleFactory.CreateProjectScopedRuleFrom(dto3).Returns(rule3);

      //WHEN
      projectAnalysis.Add(projectScopedRuleUnionDtos);

      //THEN
      Received.InOrder(() =>
      {
        ruleSet.Add(rule1);
        ruleSet.Add(rule2);
        ruleSet.Add(rule3);
      });
    }
    
    [Fact]
    public void ShouldApplyTheRulesetToSolution()
    {
      //GIVEN
      var ruleSet = Any.Instance<IProjectScopedRuleSet>();
      var projectAnalysis = new ProjectAnalysis(ruleSet, Any.Instance<IProjectScopedRuleFactory>());
      var solution = Substitute.For<ISolution>();

      var analysisReportInProgress = Any.Instance<IAnalysisReportInProgress>();

      //WHEN
      projectAnalysis.PerformOn(solution, analysisReportInProgress);

      //THEN
      solution.Received(1).Check(ruleSet, analysisReportInProgress);
    }
  }
}