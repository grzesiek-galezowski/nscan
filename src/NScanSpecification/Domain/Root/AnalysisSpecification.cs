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
    public IAnalysisReportInProgress ReportInProgress { private get; set; } = Any.Instance<IAnalysisReportInProgress>();
    public ISpecificKindOfRuleAnalysis<DependencyPathBasedRuleUnionDto> DependencyAnalysis { private get; set; } =
      Any.Instance<ISpecificKindOfRuleAnalysis<DependencyPathBasedRuleUnionDto>>();
    public ISpecificKindOfRuleAnalysis<ProjectScopedRuleUnionDto> ProjectAnalysis { private get; set; } =
      Any.Instance<ISpecificKindOfRuleAnalysis<ProjectScopedRuleUnionDto>>();
    public ISpecificKindOfRuleAnalysis<NamespaceBasedRuleUnionDto> NamespacesAnalysis { private get; set; } =
      Any.Instance<ISpecificKindOfRuleAnalysis<NamespaceBasedRuleUnionDto>>();

    public Analysis Build()
    {
      return new Analysis(
        Solution,
        ReportInProgress,
        DependencyAnalysis,
        ProjectAnalysis,
        NamespacesAnalysis);
    }
  }

  public class AnalysisSpecification
  {
    [Fact]
    public void ShouldBuildUpMetadataAndCheckRuleSetsWhenRan()
    {
      //GIVEN
      var solution = Substitute.For<ISolution>();
      var analysisReport = Any.Instance<IAnalysisReportInProgress>();
      var projectAnalysis = Substitute.For<ISpecificKindOfRuleAnalysis<ProjectScopedRuleUnionDto>>();
      var namespacesAnalysis = Substitute.For<ISpecificKindOfRuleAnalysis<NamespaceBasedRuleUnionDto>>();
      var dependencyAnalysis = Substitute.For<ISpecificKindOfRuleAnalysis<DependencyPathBasedRuleUnionDto>>();
      var analysis = new AnalysisBuilder()
      {
        ReportInProgress = analysisReport,
        ProjectAnalysis = projectAnalysis,
        NamespacesAnalysis = namespacesAnalysis,
        DependencyAnalysis = dependencyAnalysis,
        Solution = solution
      }.Build();

      //WHEN
      analysis.Run();

      //THEN
      Received.InOrder(() =>
      {
        solution.ResolveAllProjectsReferences();
        solution.BuildCache();
        dependencyAnalysis.PerformOn(solution, analysisReport);
        projectAnalysis.PerformOn(solution, analysisReport);
        namespacesAnalysis.PerformOn(solution, analysisReport);
      });
    }

    [Fact]
    public void ShouldAddAllDependencyRulesFromDtosToRuleSet()
    {
      //GIVEN
      var ruleDto1 = Any.Instance<IndependentRuleComplementDto>();
      var ruleDto2 = Any.Instance<IndependentRuleComplementDto>();
      var ruleDto3 = Any.Instance<IndependentRuleComplementDto>();
      var dependencyAnalysis = Substitute.For<ISpecificKindOfRuleAnalysis<DependencyPathBasedRuleUnionDto>>();
      var analysis = new AnalysisBuilder()
      {
        DependencyAnalysis = dependencyAnalysis
      }.Build();

      var ruleDtos = new []
      {
        DependencyPathBasedRuleUnionDto.With(ruleDto1),
        DependencyPathBasedRuleUnionDto.With(ruleDto2),
        DependencyPathBasedRuleUnionDto.With(ruleDto3)
      };

      //WHEN
      analysis.AddDependencyPathRules(ruleDtos);

      //THEN
      dependencyAnalysis.Received(1).Add(ruleDtos);
    }

    [Fact]
    public void ShouldAddAllNamespaceBasedRulesFromDtosToRuleSet()
    {
      //GIVEN
      var ruleDto1 = Any.Instance<NoCircularUsingsRuleComplementDto>();
      var ruleDto2 = Any.Instance<NoUsingsRuleComplementDto>();
      var ruleDto3 = Any.Instance<NoCircularUsingsRuleComplementDto>();
      var namespacesAnalysis = Substitute.For<ISpecificKindOfRuleAnalysis<NamespaceBasedRuleUnionDto>>();
      var analysis = new AnalysisBuilder()
      {
        NamespacesAnalysis = namespacesAnalysis,
      }.Build();

      var ruleDtos = new[]
      {
        NamespaceBasedRuleUnionDto.With(ruleDto1),
        NamespaceBasedRuleUnionDto.With(ruleDto2),
        NamespaceBasedRuleUnionDto.With(ruleDto3)
      };

      //WHEN
      analysis.AddNamespaceBasedRules(ruleDtos);

      //THEN
      namespacesAnalysis.Received(1).Add(ruleDtos);
    }
    
    [Fact]
    public void ShouldAddAllProjectScopedRulesFromDtosToRuleSet()
    {
      //GIVEN
      var ruleDto1 = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var ruleDto2 = Any.Instance<HasTargetFrameworkRuleComplementDto>();
      var ruleDto3 = Any.Instance<HasAttributesOnRuleComplementDto>();
      var projectAnalysis = Substitute.For<ISpecificKindOfRuleAnalysis<ProjectScopedRuleUnionDto>>();
      var analysis = new AnalysisBuilder
      {
        ProjectAnalysis = projectAnalysis,
      }.Build();

      var ruleDtos = new[]
      {
        ProjectScopedRuleUnionDto.With(ruleDto1),
        ProjectScopedRuleUnionDto.With(ruleDto2),
        ProjectScopedRuleUnionDto.With(ruleDto3)
      };

      //WHEN
      analysis.AddProjectScopedRules(ruleDtos);

      //THEN
      projectAnalysis.Received(1).Add(ruleDtos);
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
