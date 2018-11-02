using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification
{
  public class AnalysisSpecification
  {
    [Fact]
    public void ShouldBuildUpMetadataAndCheckRuleSetWhenRan()
    {
      //GIVEN
      var solution = Substitute.For<ISolution>();
      var pathRuleSet = Any.Instance<IPathRuleSet>();
      var analysisReport = Any.Instance<IAnalysisReportInProgress>();
      var analysis = new Analysis(
        solution, 
        pathRuleSet, 
        analysisReport, 
        Any.Instance<IRuleFactory>());

      //WHEN
      analysis.Run();

      //THEN
      Received.InOrder(() =>
      {
        solution.ResolveAllProjectsReferences(analysisReport);
        solution.BuildCache();
        solution.PrintDebugInfo();
        solution.Check(pathRuleSet, analysisReport);
      });
    }

    [Fact]
    public void ShouldAddAllRulesFromDtosToRuleSet()
    {
      //GIVEN
      var pathRuleSet = Substitute.For<IPathRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule1 = Any.Instance<IDependencyRule>();
      var rule2 = Any.Instance<IDependencyRule>();
      var rule3 = Any.Instance<IDependencyRule>();
      var ruleDto1 = Any.Instance<RuleDto>();
      var ruleDto2 = Any.Instance<RuleDto>();
      var ruleDto3 = Any.Instance<RuleDto>();
      var analysis = new Analysis(
        Any.Instance<ISolution>(),
        pathRuleSet,
        Any.Instance<IAnalysisReportInProgress>(), 
        ruleFactory);

      ruleFactory.CreateDependencyRuleFrom(ruleDto1).Returns(rule1);
      ruleFactory.CreateDependencyRuleFrom(ruleDto2).Returns(rule2);
      ruleFactory.CreateDependencyRuleFrom(ruleDto3).Returns(rule3);

      //WHEN
      analysis.AddRules(new [] {ruleDto1, ruleDto2, ruleDto3});

      //THEN
      pathRuleSet.Received(1).Add(rule1);
      pathRuleSet.Received(1).Add(rule2);
      pathRuleSet.Received(1).Add(rule3);
    }

    [Fact]
    public void ShouldReturnStringGeneratedFromAnalysisInProgressReportWhenAskedForAnalysisReport()
    {
      //GIVEN
      var analysisInProgressReport = Substitute.For<IAnalysisReportInProgress>();
      var analysis = new Analysis(
        Any.Instance<ISolution>(), 
        Any.Instance<IPathRuleSet>(), 
        analysisInProgressReport, 
        Any.Instance<IRuleFactory>());
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
      var analysis = new Analysis(
        Any.Instance<ISolution>(), 
        Any.Instance<IPathRuleSet>(), 
        reportInProgress, 
        Any.Instance<IRuleFactory>());

      reportInProgress.HasViolations().Returns(hasViolations);

      //WHEN
      var analysisReturnCode = analysis.ReturnCode;

      //THEN
      analysisReturnCode.Should().Be(expectedCode);
    }


  }
}
