using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRulesSpecification;

public class DependencyPathRuleViolationFactoryTests
{
  [Fact]
  public void ShouldCreatePathRuleViolationForPathWithViolationPathPrefix()
  {
    //GIVEN
    var reportFragmentsFormat = Substitute.For<IDependencyPathReportFragmentsFormat>();
    var factory = new DependencyPathRuleViolationFactory(reportFragmentsFormat);
    var path = Any.ReadOnlyList<IDependencyPathBasedRuleTarget>();
    var ruleDescription = Any.Instance<RuleDescription>();
    var formattedPath = Any.String();

    reportFragmentsFormat.ApplyToPath(path).Returns(formattedPath);
    
    //WHEN
    var pathRuleViolation = factory.PathRuleViolation(
      ruleDescription,
      path);
    
    //THEN
    pathRuleViolation.Should().Be(
      new RuleViolation(ruleDescription, "Violating path: ", formattedPath));
  }
}
