using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;
using NScanSpecification.Lib;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.XNSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.DependencyPathBasedRulesSpecification
{
  public class IndependentRuleSpecification
  {
    [Fact]
    public void ShouldReportOkWhenPathDoesNotContainADependingProject()
    {
      //GIVEN
      var dependencyCondition = Substitute.For<IDescribedDependencyCondition>();
      var dependingAssemblyNamePattern = Any.Pattern();
      var rule = new IndependentRule(dependencyCondition, dependingAssemblyNamePattern, Any.Instance<IDependencyPathRuleViolationFactory>());
      var report = Substitute.For<IAnalysisReportInProgress>();
      var projectDependencyPath = Substitute.For<IProjectDependencyPath>();

      projectDependencyPath.AssemblyWithNameMatching(dependingAssemblyNamePattern).Exists().Returns(false);


      //WHEN
      rule.Check(report, projectDependencyPath);

      //THEN
      XReceived.Only(() => report.FinishedEvaluatingRule(dependencyCondition.Description()));
    }

    [Fact]
    public void ShouldReportRuleViolationAndPathWhenDependencyIsDetected()
    {
      //GIVEN
      var dependencyCondition = Substitute.For<IDescribedDependencyCondition>();
      var conditionDescription = Any.Instance<RuleDescription>();
      var dependingAssemblyNamePattern = Any.Pattern();
      var ruleViolationFactory = Substitute.For<IDependencyPathRuleViolationFactory>();
      var rule = new IndependentRule(dependencyCondition, dependingAssemblyNamePattern, ruleViolationFactory);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var projectDependencyPath = Substitute.For<IProjectDependencyPath>();
      var dependingAssembly = Substitute.For<IProjectSearchResult>();
      var dependencyAssembly = Substitute.For<IProjectSearchResult>();
      var violatingPathSegment = Any.ReadOnlyList<IDependencyPathBasedRuleTarget>();
      var violation = Any.Instance<RuleViolation>();

      dependencyCondition.Description().Returns(conditionDescription);

      projectDependencyPath.AssemblyWithNameMatching(dependingAssemblyNamePattern).Returns(dependingAssembly);
      dependingAssembly.Exists().Returns(true);

      projectDependencyPath.AssemblyMatching(dependencyCondition, dependingAssembly).Returns(dependencyAssembly);
      dependencyAssembly.IsNotBefore(dependingAssembly).Returns(true);

      projectDependencyPath.SegmentBetween(dependingAssembly, dependencyAssembly).Returns(violatingPathSegment);
      ruleViolationFactory.PathRuleViolation(conditionDescription.Value, violatingPathSegment).Returns(violation);

      //WHEN
      rule.Check(report, projectDependencyPath);

      //THEN
      Received.InOrder(() =>
      {
        report.Add(violation);
        report.FinishedEvaluatingRule(conditionDescription);
      });
      
    }

    [Fact]
    public void ShouldReportRuleViolationWhenDependingProjectExistsButMatchingDependencyIsNotAfterItInThePath()
    {
      //GIVEN
      var dependencyCondition = Substitute.For<IDescribedDependencyCondition>();
      var conditionDescription = Any.Instance<RuleDescription>();
      var dependingAssemblyNamePattern = Any.Pattern();
      var rule = new IndependentRule(
        dependencyCondition, 
        dependingAssemblyNamePattern, 
        Any.Instance<IDependencyPathRuleViolationFactory>());
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
        report.FinishedEvaluatingRule(conditionDescription);
      });

    }


  }
}
