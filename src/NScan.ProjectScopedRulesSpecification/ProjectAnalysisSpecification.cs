using System.Collections.Generic;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.ProjectScopedRulesSpecification;

public class ProjectAnalysisSpecification
{
  [Fact]
  public void ShouldAddRuleToRuleSet()
  {
    //GIVEN
    var ruleSet = Substitute.For<IProjectScopedRuleSet>();
    var ruleFactory = Substitute.For<IProjectScopedRuleFactory>();
    var projectAnalysis = new ProjectAnalysis(Any.Instance<ISolutionForProjectScopedRules>(), ruleSet, ruleFactory);
    var dto1 = Any.Instance<CorrectNamespacesRuleComplementDto>();
    var dto2 = Any.Instance<HasAttributesOnRuleComplementDto>();
    var dto3 = Any.Instance<HasTargetFrameworkRuleComplementDto>();
    var dto4 = Any.Instance<HasPropertyRuleComplementDto>();
    var projectScopedRuleUnionDtos = new List<ProjectScopedRuleUnionDto>
    {
      ProjectScopedRuleUnionDto.With(dto1), 
      ProjectScopedRuleUnionDto.With(dto2), 
      ProjectScopedRuleUnionDto.With(dto3),
      ProjectScopedRuleUnionDto.With(dto4)
    };

    var rule1 = Any.Instance<IProjectScopedRule>();
    var rule2 = Any.Instance<IProjectScopedRule>();
    var rule3 = Any.Instance<IProjectScopedRule>();
    var rule4 = Any.Instance<IProjectScopedRule>();
    ruleFactory.CreateProjectScopedRuleFrom(dto1).Returns(rule1);
    ruleFactory.CreateProjectScopedRuleFrom(dto2).Returns(rule2);
    ruleFactory.CreateProjectScopedRuleFrom(dto3).Returns(rule3);
    ruleFactory.CreateProjectScopedRuleFrom(dto4).Returns(rule4);

    //WHEN
    projectAnalysis.Add(projectScopedRuleUnionDtos);

    //THEN
    Received.InOrder(() =>
    {
      ruleSet.Add(rule1);
      ruleSet.Add(rule2);
      ruleSet.Add(rule3);
      ruleSet.Add(rule4);
    });
  }
    
  [Fact]
  public void ShouldApplyTheRulesetToSolution()
  {
    //GIVEN
    var ruleSet = Any.Instance<IProjectScopedRuleSet>();
    var solution = Substitute.For<ISolutionForProjectScopedRules>();
    var projectAnalysis = new ProjectAnalysis(solution, ruleSet, Any.Instance<IProjectScopedRuleFactory>());

    var analysisReportInProgress = Any.Instance<IAnalysisReportInProgress>();

    //WHEN
    projectAnalysis.Perform(analysisReportInProgress);

    //THEN
    solution.Received(1).Check(ruleSet, analysisReportInProgress);
  }
}