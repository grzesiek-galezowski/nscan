using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.XNSubstitute.Root;
using Xunit;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class IndependentRuleSpecification
  {
    [Fact]
    public void ShouldReportOkWhenPathDoesNotContainADependingProject()
    {
      //GIVEN
      var dependencyCondition = Substitute.For<IDescribedDependencyCondition>();
      var dependingAssemblyNamePattern = AnyRoot.Root.Any.Instance<Pattern>();
      var rule = new IndependentRule(dependencyCondition, dependingAssemblyNamePattern, AnyRoot.Root.Any.Instance<IRuleViolationFactory>());
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
      var conditionDescription = AnyRoot.Root.Any.String();
      var dependingAssemblyNamePattern = AnyRoot.Root.Any.Instance<Pattern>();
      var ruleViolationFactory = Substitute.For<IRuleViolationFactory>();
      var rule = new IndependentRule(dependencyCondition, dependingAssemblyNamePattern, ruleViolationFactory);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var projectDependencyPath = Substitute.For<IProjectDependencyPath>();
      var dependingAssembly = Substitute.For<IProjectSearchResult>();
      var dependencyAssembly = Substitute.For<IProjectSearchResult>();
      var violatingPathSegment = AnyRoot.Root.Any.ReadOnlyList<IReferencedProject>();
      var violation = AnyRoot.Root.Any.Instance<RuleViolation>();

      dependencyCondition.Description().Returns(conditionDescription);

      projectDependencyPath.AssemblyWithNameMatching(dependingAssemblyNamePattern).Returns(dependingAssembly);
      dependingAssembly.Exists().Returns(true);

      projectDependencyPath.AssemblyMatching(dependencyCondition, dependingAssembly).Returns(dependencyAssembly);
      dependencyAssembly.IsNotBefore(dependingAssembly).Returns(true);

      projectDependencyPath.SegmentBetween(dependingAssembly, dependencyAssembly).Returns(violatingPathSegment);
      ruleViolationFactory.PathRuleViolation(conditionDescription, violatingPathSegment).Returns(violation);


      //WHEN
      rule.Check(report, projectDependencyPath);

      //THEN
      Received.InOrder(() =>
      {
        report.Add(violation);
        report.FinishedChecking(conditionDescription);
      });
      
    }

    [Fact]
    public void ShouldReportRuleViolationWhenDependingProjectExistsButMatchingDependencyIsNotAfterItInThePath()
    {
      //GIVEN
      var dependencyCondition = Substitute.For<IDescribedDependencyCondition>();
      var conditionDescription = AnyRoot.Root.Any.String();
      var dependingAssemblyNamePattern = AnyRoot.Root.Any.Instance<Pattern>();
      var rule = new IndependentRule(dependencyCondition, dependingAssemblyNamePattern, AnyRoot.Root.Any.Instance<IRuleViolationFactory>());
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