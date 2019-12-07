using System.Collections.Generic;
using NScan.DependencyPathBasedRules;
using NScan.Domain.Root;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NSubstitute;
using Xunit;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class DependencyAnalysisSpecification
  {
    [Fact]
    public void ShouldAddRuleToRuleSet()
    {
      //GIVEN
      var ruleSet = Substitute.For<IPathRuleSet>();
      var ruleFactory = Substitute.For<IDependencyBasedRuleFactory>();
      var analysis = new DependencyAnalysis(ruleSet, ruleFactory);
      var dto1 = AnyRoot.Root.Any.Instance<IndependentRuleComplementDto>();
      var dto2 = AnyRoot.Root.Any.Instance<IndependentRuleComplementDto>();
      var dto3 = AnyRoot.Root.Any.Instance<IndependentRuleComplementDto>();
      var projectScopedRuleUnionDtos = new List<DependencyPathBasedRuleUnionDto>()
      {
        DependencyPathBasedRuleUnionDto.With(dto1), 
        DependencyPathBasedRuleUnionDto.With(dto2), 
        DependencyPathBasedRuleUnionDto.With(dto3)
      };
      var rule1 = AnyRoot.Root.Any.Instance<IDependencyRule>();
      var rule2 = AnyRoot.Root.Any.Instance<IDependencyRule>();
      var rule3 = AnyRoot.Root.Any.Instance<IDependencyRule>();

      ruleFactory.CreateDependencyRuleFrom(dto1).Returns(rule1);
      ruleFactory.CreateDependencyRuleFrom(dto2).Returns(rule2);
      ruleFactory.CreateDependencyRuleFrom(dto3).Returns(rule3);

      //WHEN
      analysis.Add(projectScopedRuleUnionDtos);

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
      var ruleSet = AnyRoot.Root.Any.Instance<IPathRuleSet>();
      var projectAnalysis = new DependencyAnalysis(ruleSet, AnyRoot.Root.Any.Instance<IDependencyBasedRuleFactory>());
      var solution = Substitute.For<ISolution>();

      var analysisReportInProgress = AnyRoot.Root.Any.Instance<IAnalysisReportInProgress>();

      //WHEN
      projectAnalysis.PerformOn(solution, analysisReportInProgress);

      //THEN
      solution.Received(1).Check(ruleSet, analysisReportInProgress);
    }
  }
}