using LanguageExt;
using NScan.Lib;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScanSpecification.Lib;

namespace NScan.ProjectScopedRulesSpecification;

public class HasPropertyValueRuleSpecification
{
  [Fact]
  public void ShouldValidateProjectForTargetFrameworkWhenChecked()
  {
    //GIVEN
    var expectedPropertyValue = Any.Pattern();
    var rule = new HasPropertyValueRule(
      Any.String(), 
      expectedPropertyValue, 
      Any.Instance<IProjectScopedRuleViolationFactory>(), Any.Instance<RuleDescription>());
    var project = Substitute.For<IProjectScopedRuleTarget>();
    var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();

    //WHEN
    rule.Check(project, analysisReportInProgress);

    //THEN
    project.Received(1).ValidateProperty(rule, analysisReportInProgress);
  }

  [Fact]
  public void ShouldAllowGettingItsDescription()
  {
    //GIVEN
    var ruleDescription = Any.Instance<RuleDescription>();
    var expectedPropertyValue = Any.Pattern();
    var rule = new HasPropertyValueRule(
      Any.String(), 
      expectedPropertyValue, 
      Any.Instance<IProjectScopedRuleViolationFactory>(), 
      ruleDescription);

    //WHEN
    var stringRepresentation = rule.Description();

    //THEN
    stringRepresentation.Should().Be(ruleDescription);
  }

  [Fact]
  public void ShouldReportViolationWhenPropertyValueDoesNotMatchExpectation()
  {
    //GIVEN
    var propertyValue = Any.String();
    var propertyName = Any.String();
    var properties = new Dictionary<string, string>
    {
      [propertyName] = propertyValue
    }.ToHashMap();
    var expectedPropertyValue = Any.OtherThan(propertyValue);
    var violationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
    var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();
    var assemblyName = Any.Instance<AssemblyName>();
    var violation = Any.Instance<RuleViolation>();
    var description = Any.Instance<RuleDescription>();
    var rule = new HasPropertyValueRule(
      propertyName, 
      Pattern.WithoutExclusion(expectedPropertyValue), 
      violationFactory, description);

    violationFactory.ProjectScopedRuleViolation(description, $"Project {assemblyName} has {propertyName}:{propertyValue}").Returns(violation);

    //WHEN
    rule.ApplyTo(assemblyName, properties, analysisReportInProgress);

    //THEN
    analysisReportInProgress.Received(1).Add(violation);
  }

  [Fact]
  public void ShouldReportViolationWhenExpectedPropertyDoesNotExist()
  {
    //GIVEN
    var propertyName = Any.String();
    var properties = DictionaryNotContaining(propertyName);
    var expectedPropertyValue = Any.Pattern();
    var violationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
    var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();
    var assemblyName = Any.Instance<AssemblyName>();
    var violation = Any.Instance<RuleViolation>();
    var description = Any.Instance<RuleDescription>();
    var rule = new HasPropertyValueRule(
      propertyName, 
      expectedPropertyValue, 
      violationFactory, 
      description);

    violationFactory.ProjectScopedRuleViolation(description, $"Project {assemblyName} does not have {propertyName} set explicitly").Returns(violation);

    //WHEN
    rule.ApplyTo(assemblyName, properties, analysisReportInProgress);

    //THEN
    analysisReportInProgress.Received(1).Add(violation);
  }

  [Theory]
  [InlineData("abc", "abc")]
  [InlineData("abc", "ab*")]
  public void ShouldNotReportViolationWhenPropertyHasExpectedValue(string actualPropertyValue, string expectedPattern)
  {
    //GIVEN
    var propertyName = Any.String();
    var properties = new Dictionary<string, string>
    {
      [propertyName] = actualPropertyValue
    }.ToHashMap();
    var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();
    var description = Any.Instance<RuleDescription>();
    var rule = new HasPropertyValueRule(
      propertyName, 
      Pattern.WithoutExclusion(expectedPattern), 
      Any.Instance<IProjectScopedRuleViolationFactory>(), 
      description);

    //WHEN
    rule.ApplyTo(Any.Instance<AssemblyName>(), properties, analysisReportInProgress);

    //THEN
    analysisReportInProgress.DidNotReceiveWithAnyArgs().Add(default!);
  }

  private static HashMap<string, string> DictionaryNotContaining(string propertyName)
  {
    return new Dictionary<string, string>().ToHashMap();
  }
}
