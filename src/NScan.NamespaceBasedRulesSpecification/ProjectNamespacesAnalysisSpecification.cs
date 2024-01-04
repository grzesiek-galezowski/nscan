using LanguageExt;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRulesSpecification;

public class ProjectNamespacesAnalysisSpecification
{
  [Fact]
  public void ShouldAddRuleToRuleSet()
  {
    //GIVEN
    var ruleSet = Substitute.For<INamespacesBasedRuleSet>();
    var ruleFactory = Substitute.For<INamespaceBasedRuleFactory>();
    var analysis = new ProjectNamespacesAnalysis(Any.Instance<ISolutionForNamespaceBasedRules>(), ruleSet, ruleFactory);
    var dto1 = Any.Instance<NoUsingsRuleComplementDto>();
    var dto2 = Any.Instance<NoCircularUsingsRuleComplementDto>();
    var dto3 = Any.Instance<NoUsingsRuleComplementDto>();
    var projectScopedRuleUnionDtos = Seq.create(
      NamespaceBasedRuleUnionDto.With(dto1), 
      NamespaceBasedRuleUnionDto.With(dto2), 
      NamespaceBasedRuleUnionDto.With(dto3)
    );
    var rule1 = Any.Instance<INamespacesBasedRule>();
    var rule2 = Any.Instance<INamespacesBasedRule>();
    var rule3 = Any.Instance<INamespacesBasedRule>();

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
  public void ShouldBuildCacheApplyTheRulesetToSolutionWhenPerformed()
  {
    //GIVEN
    var ruleSet = Any.Instance<INamespacesBasedRuleSet>();
    var solution = Substitute.For<ISolutionForNamespaceBasedRules>();
    var projectAnalysis = new ProjectNamespacesAnalysis(solution, ruleSet, Any.Instance<INamespaceBasedRuleFactory>());

    var analysisReportInProgress = Any.Instance<IAnalysisReportInProgress>();

    //WHEN
    projectAnalysis.PerformOn(analysisReportInProgress);

    //THEN
    Received.InOrder(() =>
    {
      solution.BuildNamespacesCache();
      solution.Check(ruleSet, analysisReportInProgress);
    });
  }
}
