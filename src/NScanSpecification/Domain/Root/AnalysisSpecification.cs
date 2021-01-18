using FluentAssertions;
using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Domain.Root
{
  public class AnalysisBuilder
  {
    public IAnalysisReportInProgress ReportInProgress { private get; set; } = Any.Instance<IAnalysisReportInProgress>();
    public IDependencyAnalysis DependencyAnalysis { private get; set; } =
      Any.Instance<IDependencyAnalysis>();
    public IProjectAnalysis ProjectAnalysis { private get; set; } =
      Any.Instance<IProjectAnalysis>();
    public IProjectNamespacesAnalysis NamespacesAnalysis { private get; set; } =
      Any.Instance<IProjectNamespacesAnalysis>();
    public ISolutionForDependencyPathBasedRules SolutionForDependencyPathBasedRules { get; set; } =
      Any.Instance<ISolutionForDependencyPathBasedRules>();
    public ISolutionForProjectScopedRules SolutionForProjectScopedRules { get; set; } =
      Any.Instance<ISolutionForProjectScopedRules>();    
    public ISolutionForNamespaceBasedRules SolutionForNamespaceBasedRules { get; set; } =
      Any.Instance<ISolutionForNamespaceBasedRules>();

    public Analysis Build()
    {
      return new(ReportInProgress,
        DependencyAnalysis,
        ProjectAnalysis,
        NamespacesAnalysis);
    }
  }


  public class AnalysisSpecification
  {
    [Fact]
    public void ShouldRunAllKindsOfAnalysisWhenRan()
    {
      //GIVEN
      var analysisReport = Any.Instance<IAnalysisReportInProgress>();
      var projectAnalysis = Substitute.For<IProjectAnalysis>();
      var namespacesAnalysis = Substitute.For<IProjectNamespacesAnalysis>();
      var dependencyAnalysis = Substitute.For<IDependencyAnalysis>();
      var solutionForDependencyPathRules = Substitute.For<ISolutionForDependencyPathBasedRules>();
      var solutionForProjectScopedRules = Substitute.For<ISolutionForProjectScopedRules>();
      var solutionForNamespaceBasedRules = Substitute.For<ISolutionForNamespaceBasedRules>();
      var analysis = new AnalysisBuilder
      {
        ReportInProgress = analysisReport,
        ProjectAnalysis = projectAnalysis,
        NamespacesAnalysis = namespacesAnalysis,
        DependencyAnalysis = dependencyAnalysis,
        SolutionForDependencyPathBasedRules = solutionForDependencyPathRules,
        SolutionForProjectScopedRules = solutionForProjectScopedRules,
        SolutionForNamespaceBasedRules = solutionForNamespaceBasedRules
      }.Build();

      //WHEN
      analysis.Run();

      //THEN
      dependencyAnalysis.Received(1).Perform(analysisReport);
      projectAnalysis.Received(1).Perform(analysisReport);
      namespacesAnalysis.Received(1).PerformOn(analysisReport);
    }

    [Fact]
    public void ShouldAddAllDependencyRulesFromDtosToRuleSet()
    {
      //GIVEN
      var ruleDto1 = Any.Instance<IndependentRuleComplementDto>();
      var ruleDto2 = Any.Instance<IndependentRuleComplementDto>();
      var ruleDto3 = Any.Instance<IndependentRuleComplementDto>();
      var dependencyAnalysis = Substitute.For<IDependencyAnalysis>();
      var analysis = new AnalysisBuilder
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
      var namespacesAnalysis = Substitute.For<IProjectNamespacesAnalysis>();
      var analysis = new AnalysisBuilder
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
      var projectAnalysis = Substitute.For<IProjectAnalysis>();
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
      var analysis = new AnalysisBuilder
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
