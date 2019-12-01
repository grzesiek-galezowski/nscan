using FluentAssertions;
using NScan.DependencyPathBasedRules;
using NScan.Domain.Root;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class AnalysisBuilder
  {
    public ISolution Solution { private get; set; } = Any.Instance<ISolution>();
    public IPathRuleSet PathRuleSet { private get; set; } = Any.Instance<IPathRuleSet>();
    public IAnalysisReportInProgress ReportInProgress { private get; set; } = Any.Instance<IAnalysisReportInProgress>();
    public IRuleFactory Factory { private get; set; } = Any.Instance<IRuleFactory>();
    public IProjectScopedRuleSet ProjectScopedRuleSet { get; set; } = Any.Instance<IProjectScopedRuleSet>();
    public INamespacesBasedRuleSet ProjectNamespacesRuleSet { private get; set; } =
      Any.Instance<INamespacesBasedRuleSet>();

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
      var pathRuleSet = Any.Instance<IPathRuleSet>();
      var analysisReport = Any.Instance<IAnalysisReportInProgress>();
      var projectScopedRuleSet = Any.Instance<IProjectScopedRuleSet>();
      var namespacesBasedRuleSet = Any.Instance<INamespacesBasedRuleSet>();
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
        DependencyPathBasedRuleUnionDto.With(ruleDto1),
        DependencyPathBasedRuleUnionDto.With(ruleDto2),
        DependencyPathBasedRuleUnionDto.With(ruleDto3)
      };

      //WHEN
      analysis.AddDependencyPathRules(ruleDtos);

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
      var rule1 = Any.Instance<INamespacesBasedRule>();
      var rule2 = Any.Instance<INamespacesBasedRule>();
      var rule3 = Any.Instance<INamespacesBasedRule>();
      var ruleDto1 = Any.Instance<NoCircularUsingsRuleComplementDto>();
      var ruleDto2 = Any.Instance<NoCircularUsingsRuleComplementDto>();
      var ruleDto3 = Any.Instance<NoCircularUsingsRuleComplementDto>();
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
        NamespaceBasedRuleUnionDto.With(ruleDto1),
        NamespaceBasedRuleUnionDto.With(ruleDto2),
        NamespaceBasedRuleUnionDto.With(ruleDto3)
      };
      //WHEN
      analysis.AddNamespaceBasedRules(ruleDtos);

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
      var rule1 = Any.Instance<IProjectScopedRule>();
      var rule2 = Any.Instance<IProjectScopedRule>();
      var rule3 = Any.Instance<IProjectScopedRule>();
      var ruleDto1 = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var ruleDto2 = Any.Instance<HasTargetFrameworkRuleComplementDto>();
      var ruleDto3 = Any.Instance<HasAttributesOnRuleComplementDto>();
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
        ProjectScopedRuleUnionDto.With(ruleDto1),
        ProjectScopedRuleUnionDto.With(ruleDto2),
        ProjectScopedRuleUnionDto.With(ruleDto3)
      };

      //WHEN
      analysis.AddProjectScopedRules(ruleDtos);

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
