using System;
using FluentAssertions;
using MyTool;
using MyTool.App;
using MyTool.CompositionRoot;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyToolSpecification
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
      var analysis = new Analysis(solution, pathRuleSet, analysisReport, Any.Instance<IRuleFactory>());

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
    public void ShouldAddDirectIndependentProjectRuleToPathRuleSet()
    {
      //GIVEN
      var pathRuleSet = Substitute.For<IPathRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule = Any.Instance<IDependencyRule>();
      var analysis = new Analysis(
        Any.Instance<ISolution>(), 
        pathRuleSet, 
        Any.Instance<IAnalysisReportInProgress>(), ruleFactory);
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();

      ruleFactory.CreateDirectIndependentOfProjectRule(dependingId, dependencyId).Returns(rule);

      //WHEN
      analysis.DirectIndependentOfProject(dependingId, dependencyId);

      //THEN
      pathRuleSet.Received(1).Add(rule);
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
  }
}
