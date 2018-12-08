using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using TddXt.NScan.RuleInputData;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class AnalysisBuilder
  {
    public ISolution Solution { private get; set; } = Any.Instance<ISolution>();
    public IPathRuleSet PathRuleSet { private get; set; } = Any.Instance<IPathRuleSet>();
    public IAnalysisReportInProgress ReportInProgress { private get; set; } = Any.Instance<IAnalysisReportInProgress>();
    public IRuleFactory Factory { private get; set; } = Any.Instance<IRuleFactory>();
    public IProjectScopedRuleSet ProjectScopedRuleSet { get; set; } = Any.Instance<IProjectScopedRuleSet>();

    public Analysis Build()
    {
      return new Analysis(Solution, PathRuleSet, ProjectScopedRuleSet, ReportInProgress, Factory);
    }
  }

  public class AnalysisSpecification
  {
    [Fact]
    public void ShouldBuildUpMetadataAndCheckRuleSetsWhenRan()
    {
      //GIVEN
      var solution = Substitute.For<ISolution>();
      var pathRuleSet = Any.Instance<IPathRuleSet>();
      var analysisReport = Any.Instance<IAnalysisReportInProgress>();
      var projectScopedRuleSet = Any.Instance<IProjectScopedRuleSet>();
      var analysis = new AnalysisBuilder()
      {
        ReportInProgress = analysisReport,
        PathRuleSet = pathRuleSet,
        ProjectScopedRuleSet = projectScopedRuleSet,
        Solution = solution
      }.Build();

      //WHEN
      analysis.Run();

      //THEN
      Received.InOrder(() =>
      {
        solution.ResolveAllProjectsReferences();
        solution.BuildCache();
        solution.PrintDebugInfo();
        solution.Check(pathRuleSet, analysisReport);
        solution.Check(projectScopedRuleSet, analysisReport);
      });
    }

    [Fact]
    public void ShouldAddAllDependencyRulesFromDtosToRuleSet()
    {
      //GIVEN
      var pathRuleSet = Substitute.For<IPathRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule1 = Any.Instance<IDependencyRule>();
      var rule2 = Any.Instance<IDependencyRule>();
      var rule3 = Any.Instance<IDependencyRule>();
      var ruleDto1 = Any.Instance<IndependentRuleComplementDto>();
      var ruleDto2 = Any.Instance<IndependentRuleComplementDto>();
      var ruleDto3 = Any.Instance<IndependentRuleComplementDto>();
      var analysis = new AnalysisBuilder()
      {
        PathRuleSet = pathRuleSet,
        Factory = ruleFactory
      }.Build();

      ruleFactory.CreateDependencyRuleFrom(ruleDto1).Returns(rule1);
      ruleFactory.CreateDependencyRuleFrom(ruleDto2).Returns(rule2);
      ruleFactory.CreateDependencyRuleFrom(ruleDto3).Returns(rule3);
      var ruleDtos = new []
      {
        RuleUnionDto.With(ruleDto1),
        RuleUnionDto.With(ruleDto2),
        RuleUnionDto.With(ruleDto3)
      };

      //WHEN
      analysis.AddRules(ruleDtos);

      //THEN
      pathRuleSet.Received(1).Add(rule1);
      pathRuleSet.Received(1).Add(rule2);
      pathRuleSet.Received(1).Add(rule3);
    }

    [Fact]
    public void ShouldAddAllProjectScopedRulesFromDtosToRuleSet()
    {
      //GIVEN
      var projectScopedRuleSet = Substitute.For<IProjectScopedRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule1 = Any.Instance<IProjectScopedRule>();
      var rule2 = Any.Instance<IProjectScopedRule>();
      var rule3 = Any.Instance<IProjectScopedRule>();
      var ruleDto1 = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var ruleDto2 = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var ruleDto3 = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var analysis = new AnalysisBuilder()
      {
        ProjectScopedRuleSet = projectScopedRuleSet,
        Factory = ruleFactory
      }.Build();

      ruleFactory.CreateProjectScopedRuleFrom(ruleDto1).Returns(rule1);
      ruleFactory.CreateProjectScopedRuleFrom(ruleDto2).Returns(rule2);
      ruleFactory.CreateProjectScopedRuleFrom(ruleDto3).Returns(rule3);
      
      var ruleDtos = new[]
      {
        RuleUnionDto.With(ruleDto1),
        RuleUnionDto.With(ruleDto2),
        RuleUnionDto.With(ruleDto3)
      };

      //WHEN
      analysis.AddRules(ruleDtos);

      //THEN
      projectScopedRuleSet.Received(1).Add(rule1);
      projectScopedRuleSet.Received(1).Add(rule2);
      projectScopedRuleSet.Received(1).Add(rule3);
    }


    [Fact]
    public void ShouldReturnStringGeneratedFromAnalysisInProgressReportWhenAskedForAnalysisReport()
    {
      //GIVEN
      var analysisInProgressReport = Substitute.For<IAnalysisReportInProgress>();
      var analysis = new AnalysisBuilder()
      {
        ReportInProgress = analysisInProgressReport
      }.Build();
      var reportStringGeneratedFromInProgressReport = Any.String();

      analysisInProgressReport.AsString().Returns(reportStringGeneratedFromInProgressReport);

      //WHEN
      var analysisReportString = analysis.Report;

      //THEN
      analysisReportString.Should().Be(reportStringGeneratedFromInProgressReport);
    }

    [Theory]
    [InlineData(false, 0)] //todo extract to constant
    [InlineData(true, -1)]
    public void ShouldReturnSuccessWhenNoViolationAreInReport(bool hasViolations, int expectedCode)
    {
      //GIVEN
      var reportInProgress = Substitute.For<IAnalysisReportInProgress>();
      var analysis = new AnalysisBuilder
      {
        ReportInProgress = reportInProgress
      }.Build();

      reportInProgress.HasViolations().Returns(hasViolations);

      //WHEN
      var analysisReturnCode = analysis.ReturnCode;

      //THEN
      analysisReturnCode.Should().Be(expectedCode);
    }
  }
}
