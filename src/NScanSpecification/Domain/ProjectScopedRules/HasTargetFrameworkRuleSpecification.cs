using System;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.ProjectScopedRules
{
  public class HasTargetFrameworkRuleSpecification
  {
    [Fact]
    public void ShouldDoNothingWhenProjectDoesNotMatchAssemblyName()
    {
      //GIVEN
      var projectAssemblyNamePattern = Any.Pattern();
      var rule = new HasTargetFrameworkRule(projectAssemblyNamePattern, Any.String(), Any.Instance<IProjectScopedRuleViolationFactory>(), Any.String());
      var project = Substitute.For<IProjectScopedRuleTarget>();
      var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();

      project.HasProjectAssemblyNameMatching(projectAssemblyNamePattern).Returns(false);

      //WHEN
      rule.Check(project, analysisReportInProgress);

      //THEN
      analysisReportInProgress.ReceivedNothing(); //bug add method to XNSubstitute - ReceivedNoCommands() to check for void or Task
    }
    
    [Fact]
    public void ShouldValidateProjectForTargetFrameworkWhenProjectMatchesAssemblyNameAndTargetFramework()
    {
      //GIVEN
      var projectAssemblyNamePattern = Any.Pattern();
      var targetFramework = Any.String();
      var rule = new HasTargetFrameworkRule(projectAssemblyNamePattern, targetFramework, Any.Instance<IProjectScopedRuleViolationFactory>(), Any.String());
      var project = Substitute.For<IProjectScopedRuleTarget>();
      var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();

      project.HasProjectAssemblyNameMatching(projectAssemblyNamePattern).Returns(true);

      //WHEN
      rule.Check(project, analysisReportInProgress);

      //THEN
      project.Received(1).ValidateTargetFrameworkWith((ITargetFrameworkCheck)rule, analysisReportInProgress);
    }

    [Fact]
    public void ShouldGiveItsDescriptionWhenConvertedToString()
    {
        //GIVEN
        var ruleDescription = Any.String();
        var targetFramework = Any.String();
        var rule = new HasTargetFrameworkRule(
          Any.Pattern(), 
          targetFramework, 
          Any.Instance<IProjectScopedRuleViolationFactory>(), 
          ruleDescription);

        //WHEN
        var stringRepresentation = rule.ToString();

        //THEN
        stringRepresentation.Should().Be(ruleDescription + " " + targetFramework);
    }


        [Fact]
        public void ShouldReportNothingWhenProjectMatchesAssemblyNameAndTargetFramework()
        {
          //GIVEN
          var projectAssemblyNamePattern = Any.Pattern();
          var targetFramework = Any.String();
          var expectedTargetFramework = Any.String();
          var violationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
          var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();
          var ruleDescription = Any.String();
          var assemblyName = Any.String();
          var violation = Any.Instance<RuleViolation>();
          var rule = new HasTargetFrameworkRule(
            projectAssemblyNamePattern, expectedTargetFramework, violationFactory, ruleDescription);

          violationFactory.ProjectScopedRuleViolation(
            ruleDescription + " " + expectedTargetFramework, $"Project {assemblyName} has target framework {targetFramework}").Returns(violation);

          //WHEN
          rule.ApplyTo(assemblyName, targetFramework, analysisReportInProgress);

          //THEN
          analysisReportInProgress.Received(1).Add(violation);
        }

    }

}