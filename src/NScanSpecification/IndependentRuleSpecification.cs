using System.Collections.Generic;
using FluentAssertions;
using GlobExpressions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using TddXt.NScan.Domain;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification
{
  public class IndependentRuleSpecification
  {
    //todo what about the situation where there is only root?

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
      XReceived.Only(() => report.Ok(dependencyCondition.Description()));
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
      XReceived.Only(() =>
      {
        report.Violation(conditionDescription, violatingPathSegment);
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
        report.Ok(conditionDescription);
      });

    }


  }
}