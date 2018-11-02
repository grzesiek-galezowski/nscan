using FluentAssertions;
using GlobExpressions;
using NSubstitute;
using TddXt.AnyRoot;
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
    public void ShouldAddIndependentProjectRuleToPathRuleSet()
    {
      //GIVEN
      var pathRuleSet = Substitute.For<IPathRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule = Any.Instance<IDependencyRule>();
      var dependingNamePattern = Any.Instance<Glob>();
      var dependencyNamePattern = Any.Instance<Glob>();
      var analysis = new Analysis(
        Any.Instance<ISolution>(), 
        pathRuleSet, 
        Any.Instance<IAnalysisReportInProgress>(), ruleFactory);

      ruleFactory.CreateIndependentOfProjectRule(dependingNamePattern, dependencyNamePattern).Returns(rule);

      //WHEN
      analysis.IndependentOfProject(dependingNamePattern, dependencyNamePattern);

      //THEN
      pathRuleSet.Received(1).Add(rule);
    }

    [Fact]
    public void ShouldAddIndependentPackageRuleToPathRuleSet()
    {
      //GIVEN
      var pathRuleSet = Substitute.For<IPathRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule = Any.Instance<IDependencyRule>();
      var analysis = new Analysis(
        Any.Instance<ISolution>(),
        pathRuleSet,
        Any.Instance<IAnalysisReportInProgress>(), ruleFactory);
      var dependingNamePattern = Any.Instance<Glob>();
      var packageNamePattern = Any.Instance<Glob>();

      ruleFactory.CreateIndependentOfPackageRule(dependingNamePattern, packageNamePattern).Returns(rule);

      //WHEN
      analysis.IndependentOfPackage(dependingNamePattern, packageNamePattern);

      //THEN
      pathRuleSet.Received(1).Add(rule);
    }

    [Fact]
    public void ShouldAddIndependentOfAssemblyRuleToPathRuleSetWhenAsked()
    {
      //GIVEN
      var pathRuleSet = Substitute.For<IPathRuleSet>();
      var ruleFactory = Substitute.For<IRuleFactory>();
      var rule = Any.Instance<IDependencyRule>();
      var analysis = new Analysis(
        Any.Instance<ISolution>(),
        pathRuleSet,
        Any.Instance<IAnalysisReportInProgress>(), ruleFactory);
      var dependingNamePattern = Any.Instance<Glob>();
      var assemblyNamePattern = Any.Instance<Glob>();

      ruleFactory.CreateIndependentOfAssemblyRule(dependingNamePattern, assemblyNamePattern).Returns(rule);

      //WHEN
      analysis.IndependentOfAssembly(dependingNamePattern, assemblyNamePattern);

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
