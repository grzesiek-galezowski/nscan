using System.Collections.Generic;
using NScan.Domain.Root;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NSubstitute;
using Xunit;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class ProjectNamespacesAnalysisSpecification
  {
    [Fact]
    public void ShouldAddRuleToRuleSet()
    {
      //GIVEN
      var ruleSet = Substitute.For<INamespacesBasedRuleSet>();
      var ruleFactory = Substitute.For<INamespaceBasedRuleFactory>();
      var analysis = new ProjectNamespacesAnalysis(ruleSet, ruleFactory);
      var dto1 = AnyRoot.Root.Any.Instance<NoUsingsRuleComplementDto>();
      var dto2 = AnyRoot.Root.Any.Instance<NoCircularUsingsRuleComplementDto>();
      var dto3 = AnyRoot.Root.Any.Instance<NoUsingsRuleComplementDto>();
      var projectScopedRuleUnionDtos = new List<NamespaceBasedRuleUnionDto>()
      {
        NamespaceBasedRuleUnionDto.With(dto1), 
        NamespaceBasedRuleUnionDto.With(dto2), 
        NamespaceBasedRuleUnionDto.With(dto3)
      };
      var rule1 = AnyRoot.Root.Any.Instance<INamespacesBasedRule>();
      var rule2 = AnyRoot.Root.Any.Instance<INamespacesBasedRule>();
      var rule3 = AnyRoot.Root.Any.Instance<INamespacesBasedRule>();

      ruleFactory.CreateNamespacesBasedRuleFrom(dto1).Returns(rule1);
      ruleFactory.CreateNamespacesBasedRuleFrom(dto2).Returns(rule2);
      ruleFactory.CreateNamespacesBasedRuleFrom(dto3).Returns(rule3);

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
      var ruleSet = AnyRoot.Root.Any.Instance<INamespacesBasedRuleSet>();
      var projectAnalysis = new ProjectNamespacesAnalysis(ruleSet, AnyRoot.Root.Any.Instance<INamespaceBasedRuleFactory>());
      var solution = Substitute.For<ISolution>();

      var analysisReportInProgress = AnyRoot.Root.Any.Instance<IAnalysisReportInProgress>();

      //WHEN
      projectAnalysis.PerformOn(solution, analysisReportInProgress);

      //THEN
      solution.Received(1).Check(ruleSet, analysisReportInProgress);
    }
  }
}