using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class IndependentRuleSpecification
  {
    [Fact]
    public void ShouldReportOkWhenPathDoesNotContainADependingProject()
    {
      //GIVEN
      var dependencyCondition = Substitute.For<IDescribedDependencyCondition>();
      var dependingAssemblyNamePattern = Any.Instance<Pattern>();
      var rule = new IndependentRule(dependencyCondition, dependingAssemblyNamePattern);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var projectDependencyPath = Substitute.For<IProjectDependencyPath>();

      projectDependencyPath.AssemblyWithNameMatching(dependingAssemblyNamePattern).Exists().Returns(false);


      //WHEN
      rule.Check(report, projectDependencyPath);

      //THEN
      XReceived.Only(() => report.FinishedChecking(dependencyCondition.Description()));
    }

    [Fact]
    public void ShouldReportRuleViolationAndPathWhenDependencyIsDetected()
    {
      //GIVEN
      var dependencyCondition = Substitute.For<IDescribedDependencyCondition>();
      var conditionDescription = Any.String();
      var dependingAssemblyNamePattern = Any.Instance<Pattern>();
      var rule = new IndependentRule(dependencyCondition, dependingAssemblyNamePattern);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var projectDependencyPath = Substitute.For<IProjectDependencyPath>();
      var dependingAssembly = Substitute.For<IProjectSearchResult>();
      var dependencyAssembly = Substitute.For<IProjectSearchResult>();
      var violatingPathSegment = Any.ReadOnlyList<IReferencedProject>();

      dependencyCondition.Description().Returns(conditionDescription);

      projectDependencyPath.AssemblyWithNameMatching(dependingAssemblyNamePattern).Returns(dependingAssembly);
      dependingAssembly.Exists().Returns(true);

      projectDependencyPath.AssemblyMatching(dependencyCondition, dependingAssembly).Returns(dependencyAssembly);
      dependencyAssembly.IsNotBefore(dependingAssembly).Returns(true);

      projectDependencyPath.SegmentBetween(dependingAssembly, dependencyAssembly).Returns(violatingPathSegment);

      //WHEN
      rule.Check(report, projectDependencyPath);

      //THEN
      Received.InOrder(() =>
      {
        report.PathViolation(conditionDescription, violatingPathSegment);
        report.FinishedChecking(conditionDescription);
      });
      
    }

    [Fact]
    public void ShouldReportRuleViolationWhenDependingProjectExistsButMatchingDependencyIsNotAfterItInThePath()
    {
      //GIVEN
      var dependencyCondition = Substitute.For<IDescribedDependencyCondition>();
      var conditionDescription = Any.String();
      var dependingAssemblyNamePattern = Any.Instance<Pattern>();
      var rule = new IndependentRule(dependencyCondition, dependingAssemblyNamePattern);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var projectDependencyPath = Substitute.For<IProjectDependencyPath>();
      var dependingAssembly = Substitute.For<IProjectSearchResult>();
      var dependencyAssembly = Substitute.For<IProjectSearchResult>();

      dependencyCondition.Description().Returns(conditionDescription);

      projectDependencyPath.AssemblyWithNameMatching(dependingAssemblyNamePattern).Returns(dependingAssembly);
      dependingAssembly.Exists().Returns(true);

      projectDependencyPath.AssemblyMatching(dependencyCondition, dependingAssembly).Returns(dependencyAssembly);
      dependencyAssembly.IsNotBefore(dependingAssembly).Returns(false);

      //WHEN
      rule.Check(report, projectDependencyPath);

      //THEN
      XReceived.Only(() =>
      {
        report.FinishedChecking(conditionDescription);
      });

    }


  }
}