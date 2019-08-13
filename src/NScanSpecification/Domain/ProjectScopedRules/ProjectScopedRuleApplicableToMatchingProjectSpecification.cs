﻿using FluentAssertions;
using NSubstitute;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.ProjectScopedRules
{
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
      innerRule.Received(1).Check(target, report);
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
    public void ShouldReturnItsInnerRuleStringRepresentationWhenConvertedToString()
    {
      //GIVEN
      var innerRule = Any.Instance<IProjectScopedRule>();
      var rule = new ProjectScopedRuleApplicableToMatchingProject(Any.Instance<Pattern>(), innerRule);

      //WHEN
      var stringRepresentation = rule.ToString();

      //THEN
      stringRepresentation.Should().Be(innerRule.ToString());
    }
  }
}