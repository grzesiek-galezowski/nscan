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
      var analysisReport = Any.Instance<IAnalysisInProgressReport>();
      var analysis = new Analysis(solution, pathRuleSet, analysisReport);

      //WHEN
      analysis.Run();

      //THEN
      Received.InOrder(() =>
      {
        solution.ResolveAllProjectsReferences(analysisReport);
        solution.BuildCaches();
        solution.PrintDebugInfo();
        solution.Check(pathRuleSet, analysisReport);
      });
    }

    [Fact]
    public void ShouldAddDirectIndependentProjectRuleToPathRuleSet()
    {
      //GIVEN
      var pathRuleSet = Substitute.For<IPathRuleSet>();
      var analysis = new Analysis(
        Any.Instance<ISolution>(), 
        pathRuleSet, 
        Any.Instance<IAnalysisInProgressReport>());
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();

      //WHEN
      analysis.DirectIndependentOfProject(dependingId, dependencyId);

      //THEN
      pathRuleSet.Received(1)
        .AddDirectIndependentOfProjectRule(dependingId, dependencyId);
    }

    [Fact]
    public void ShouldReturnStringGeneratedFromAnalysisInProgressReportWhenAskedForAnalysisReport()
    {
      //GIVEN
      var analysisInProgressReport = Substitute.For<IAnalysisInProgressReport>();
      var analysis = new Analysis(
        Any.Instance<ISolution>(), 
        Any.Instance<IPathRuleSet>(), 
        analysisInProgressReport);
      var reportStringGeneratedFromInProgressReport = Any.String();

      analysisInProgressReport.AsString().Returns(reportStringGeneratedFromInProgressReport);

      //WHEN
      var analysisReportString = analysis.Report;

      //THEN
      analysisReportString.Should().Be(reportStringGeneratedFromInProgressReport);
    }
  }
}
