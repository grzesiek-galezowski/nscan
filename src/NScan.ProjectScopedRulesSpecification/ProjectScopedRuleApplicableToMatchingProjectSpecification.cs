using FluentAssertions;
using NScan.Lib;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScanSpecification.Lib;
using NSubstitute;
using TddXt.XNSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.ProjectScopedRulesSpecification;

public class ProjectScopedRuleApplicableToMatchingProjectSpecification
{
  [Fact]
  public void ShouldDelegateToInnerRuleWhenProjectMatchesExpectedPattern()
  {
    //GIVEN
    var innerRule = Substitute.For<IProjectScopedRule>();
    var projectPattern = Any.Pattern();
    var rule = new ProjectScopedRuleApplicableToMatchingProject(projectPattern, innerRule);
    var target = Substitute.For<IProjectScopedRuleTarget>();
    var report = Any.Instance<IAnalysisReportInProgress>();

    target.HasProjectAssemblyNameMatching(projectPattern).Returns(true);

    //WHEN
    rule.Check(target, report);

    //THEN
    Received.InOrder(() =>
    {
      target.AddInfoAboutMatchingPatternTo(report);
      innerRule.Check(target, report);

    });
  }
    
  [Fact]
  public void ShouldNotDelegateToInnerRuleWhenProjectDoesNotMatchExpectedPattern()
  {
    //GIVEN
    var innerRule = Substitute.For<IProjectScopedRule>();
    var projectPattern = Any.Pattern();
    var rule = new ProjectScopedRuleApplicableToMatchingProject(projectPattern, innerRule);
    var target = Substitute.For<IProjectScopedRuleTarget>();
    var report = Any.Instance<IAnalysisReportInProgress>();

    target.HasProjectAssemblyNameMatching(projectPattern).Returns(false);

    //WHEN
    rule.Check(target, report);

    //THEN
    innerRule.ReceivedNothing();
  }
    
  [Fact]
  public void ShouldReturnItsInnerRuleDescriptionWhenAskedForOne()
  {
    //GIVEN
    var innerRule = Any.Instance<IProjectScopedRule>();
    var rule = new ProjectScopedRuleApplicableToMatchingProject(
      Any.Instance<Pattern>(), 
      innerRule);

    //WHEN
    var description = rule.Description();

    //THEN
    description.Should().Be(innerRule.Description());
  }
}