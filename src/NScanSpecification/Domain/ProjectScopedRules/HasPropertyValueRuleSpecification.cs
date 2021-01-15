using System.Collections.Generic;
using FluentAssertions;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.ProjectScopedRules
{
  public class HasPropertyValueRuleSpecification
  {
    [Fact]
    public void ShouldValidateProjectForTargetFrameworkWhenChecked()
    {
      //GIVEN
      var rule = new HasPropertyValueRule(
        Any.String(), 
        Any.String(), 
        Any.Instance<IProjectScopedRuleViolationFactory>(), 
        Any.String());
      var project = Substitute.For<IProjectScopedRuleTarget>();
      var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();

      //WHEN
      rule.Check(project, analysisReportInProgress);

      //THEN
      project.Received(1).ValidateProperty(rule, analysisReportInProgress);
    }

    [Fact]
    public void ShouldGiveItsDescriptionWhenConvertedToString()
    {
      //GIVEN
      var ruleDescription = Any.String();
      var rule = new HasPropertyValueRule(
        Any.String(), 
        Any.String(), 
        Any.Instance<IProjectScopedRuleViolationFactory>(), ruleDescription);

      //WHEN
      var stringRepresentation = rule.ToString();

      //THEN
      stringRepresentation.Should().Be(ruleDescription);
    }

    [Fact]
    public void ShouldReportNothingWhenProjectMatchesAssemblyNameAndTargetFramework()
    {
      //GIVEN
      var propertyValue = Any.String();
      var propertyName = Any.String();
      var properties = new Dictionary<string, string>
      {
        //bug redundancy in const declaration
        [propertyName] = propertyValue
      };
      var expectedPropertyValue = Any.String();
      var violationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
      var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();
      var ruleDescription = Any.String();
      var assemblyName = Any.String();
      var violation = Any.Instance<RuleViolation>();
      var rule = new HasPropertyValueRule(
        propertyName, 
        expectedPropertyValue, 
        violationFactory, 
        ruleDescription);

      violationFactory.ProjectScopedRuleViolation(
        ruleDescription, $"Project {assemblyName} has {propertyName} {propertyValue}").Returns(violation);

      //WHEN
      rule.ApplyTo(assemblyName, properties, analysisReportInProgress);

      //THEN
      analysisReportInProgress.Received(1).Add(violation);
    }
  }
}
