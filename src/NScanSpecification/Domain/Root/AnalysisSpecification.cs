using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;
using Xunit;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class AnalysisBuilder
  {
    public ISolution Solution { private get; set; } = AnyRoot.Root.Any.Instance<ISolution>();
    public IPathRuleSet PathRuleSet { private get; set; } = AnyRoot.Root.Any.Instance<IPathRuleSet>();
    public IAnalysisReportInProgress ReportInProgress { private get; set; } = AnyRoot.Root.Any.Instance<IAnalysisReportInProgress>();
    public IRuleFactory Factory { private get; set; } = AnyRoot.Root.Any.Instance<IRuleFactory>();
    public IProjectScopedRuleSet ProjectScopedRuleSet { get; set; } = AnyRoot.Root.Any.Instance<IProjectScopedRuleSet>();
    public INamespacesBasedRuleSet ProjectNamespacesRuleSet { private get; set; }

    public Analysis Build()
    {
      return new Analysis(Solution, PathRuleSet, ProjectScopedRuleSet, ProjectNamespacesRuleSet, ReportInProgress, Factory);
    }
  }

  public class AnalysisSpecification
  {
    [Fact]
    public void ShouldBuildUpMetadataAndCheckRuleSetsWhenRan()
    {
      //GIVEN
      var solution = Substitute.For<ISolution>();
      var pathRuleSet = AnyRoot.Root.Any.Instance<IPathRuleSet>();
      var analysisReport = AnyRoot.Root.Any.Instance<IAnalysisReportInProgress>();
      var projectScopedRuleSet = AnyRoot.Root.Any.Instance<IProjectScopedRuleSet>();
      var namespacesBasedRuleSet = AnyRoot.Root.Any.Instance<INamespacesBasedRuleSet>();
      var analysis = new AnalysisBuilder()
      {
        ReportInProgress = analysisReport,
        PathRuleSet = pathRuleSet,
        ProjectScopedRuleSet = projectScopedRuleSet,
        ProjectNamespacesRuleSet = namespacesBasedRuleSet,
        Solution = solution
      }.Build();

      //WHEN
      analysis.Run();

      //THEN
      Received.InOrder(() =>
      {
        solution.ResolveAllProjectsReferences();
        solution.BuildCache();
        solution.Check(pathRuleSet, analysisReport);
        solution.Check(projectScopedRuleSet, analysisReport);
        solution.Check(namespacesBasedRuleSet, analysisReport);
      });
    }

    [Fact]
    public void ShouldAddAllDependencyRulesFromDtosToRuleSet()
    {
      //GIVEN
      var pathRuleSet = Substitute.For<IPathRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule1 = AnyRoot.Root.Any.Instance<IDependencyRule>();
      var rule2 = AnyRoot.Root.Any.Instance<IDependencyRule>();
      var rule3 = AnyRoot.Root.Any.Instance<IDependencyRule>();
      var ruleDto1 = AnyRoot.Root.Any.Instance<IndependentRuleComplementDto>();
      var ruleDto2 = AnyRoot.Root.Any.Instance<IndependentRuleComplementDto>();
      var ruleDto3 = AnyRoot.Root.Any.Instance<IndependentRuleComplementDto>();
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
    public void ShouldAddAllNamespaceBasedRulesFromDtosToRuleSet()
    {
      //GIVEN
      var namespaceBasedRuleSet = Substitute.For<INamespacesBasedRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule1 = AnyRoot.Root.Any.Instance<INamespacesBasedRule>();
      var rule2 = AnyRoot.Root.Any.Instance<INamespacesBasedRule>();
      var rule3 = AnyRoot.Root.Any.Instance<INamespacesBasedRule>();
      var ruleDto1 = AnyRoot.Root.Any.Instance<NoCircularUsingsRuleComplementDto>();
      var ruleDto2 = AnyRoot.Root.Any.Instance<NoCircularUsingsRuleComplementDto>();
      var ruleDto3 = AnyRoot.Root.Any.Instance<NoCircularUsingsRuleComplementDto>();
      var analysis = new AnalysisBuilder()
      {
        ProjectNamespacesRuleSet = namespaceBasedRuleSet,
        Factory = ruleFactory
      }.Build();

      ruleFactory.CreateNamespacesBasedRuleFrom(ruleDto1).Returns(rule1);
      ruleFactory.CreateNamespacesBasedRuleFrom(ruleDto2).Returns(rule2);
      ruleFactory.CreateNamespacesBasedRuleFrom(ruleDto3).Returns(rule3);
      
      var ruleDtos = new[]
      {
        RuleUnionDto.With(ruleDto1),
        RuleUnionDto.With(ruleDto2),
        RuleUnionDto.With(ruleDto3)
      };

      //WHEN
      analysis.AddRules(ruleDtos);

      //THEN
      namespaceBasedRuleSet.Received(1).Add(rule1);
      namespaceBasedRuleSet.Received(1).Add(rule2);
      namespaceBasedRuleSet.Received(1).Add(rule3);
    }

    [Fact]
    public void ShouldAddAllProjectScopedRulesFromDtosToRuleSet()
    {
      //GIVEN
      var projectScopedRuleSet = Substitute.For<IProjectScopedRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule1 = AnyRoot.Root.Any.Instance<IProjectScopedRule>();
      var rule2 = AnyRoot.Root.Any.Instance<IProjectScopedRule>();
      var rule3 = AnyRoot.Root.Any.Instance<IProjectScopedRule>();
      var ruleDto1 = AnyRoot.Root.Any.Instance<CorrectNamespacesRuleComplementDto>();
      var ruleDto2 = AnyRoot.Root.Any.Instance<CorrectNamespacesRuleComplementDto>();
      var ruleDto3 = AnyRoot.Root.Any.Instance<CorrectNamespacesRuleComplementDto>();
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
      var reportStringGeneratedFromInProgressReport = AnyRoot.Root.Any.String();

      analysisInProgressReport.AsString().Returns(reportStringGeneratedFromInProgressReport);

      //WHEN
      var analysisReportString = analysis.Report;

      //THEN
      analysisReportString.Should().Be(reportStringGeneratedFromInProgressReport);
    }

    [Theory]
    [InlineData(Analysis.ReturnCodeAnalysisFailed, -1)]
    [InlineData(Analysis.ReturnCodeOk, 0)]
    public void ShouldDefineReturnCodes(int value, int expected)
    {
      value.Should().Be(expected);
    }

    [Theory]
    [InlineData(false, Analysis.ReturnCodeOk)]
    [InlineData(true, Analysis.ReturnCodeAnalysisFailed)]
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
