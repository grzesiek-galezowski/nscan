namespace NScan.DependencyPathBasedRules;

public class DependencyPathRuleViolationFactory(IDependencyPathReportFragmentsFormat reportFragmentsFormat)
  : IDependencyPathRuleViolationFactory
{
  public RuleViolation PathRuleViolation(
    RuleDescription description,
    Seq<IDependencyPathBasedRuleTarget> violationPath)
  {
    return RuleViolation.Create(
      description, 
      "Violating path: ", 
      reportFragmentsFormat.ApplyToPath(violationPath));
  }
}
