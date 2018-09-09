using FluentAssertions;
using MyTool.App;
using MyTool.CompositionRoot;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using Xunit;

namespace MyTool
{
  public class AnalysisSpecification
  {
    [Fact]
    public void ShouldBuildUpMetadataAndCheckRuleSetWhenRan()
    {
      //GIVEN
      var solution = Substitute.For<ISolution>();
      var pathRuleSet = Root.Any.Instance<IPathRuleSet>();
      var analysisReport = Root.Any.Instance<IAnalysisReportInProgress>();
      var analysis = new Analysis(solution, pathRuleSet, analysisReport, Root.Any.Instance<IRuleFactory>());

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
      var rule = Root.Any.Instance<IDependencyRule>();
      var analysis = new Analysis(
        Root.Any.Instance<ISolution>(), 
        pathRuleSet, 
        Root.Any.Instance<IAnalysisReportInProgress>(), ruleFactory);
      var dependingId = Root.Any.ProjectId();
      var dependencyId = Root.Any.ProjectId();

      ruleFactory.CreateIndependentOfProjectRule(dependingId, dependencyId).Returns(rule);

      //WHEN
      analysis.IndependentOfProject(dependingId, dependencyId);

      //THEN
      pathRuleSet.Received(1).Add(rule);
    }

    [Fact]
    public void ShouldReturnStringGeneratedFromAnalysisInProgressReportWhenAskedForAnalysisReport()
    {
      //GIVEN
      var analysisInProgressReport = Substitute.For<IAnalysisReportInProgress>();
      var analysis = new Analysis(
        Root.Any.Instance<ISolution>(), 
        Root.Any.Instance<IPathRuleSet>(), 
        analysisInProgressReport, 
        Root.Any.Instance<IRuleFactory>());
      var reportStringGeneratedFromInProgressReport = Root.Any.String();

      analysisInProgressReport.AsString().Returns(reportStringGeneratedFromInProgressReport);

      //WHEN
      var analysisReportString = analysis.Report;

      //THEN
      analysisReportString.Should().Be(reportStringGeneratedFromInProgressReport);
    }
  }
}
