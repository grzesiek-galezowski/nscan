namespace NScan.DependencyPathBasedRules;

public interface IDependencyPathRuleViolationFactory
{
  RuleViolation PathRuleViolation(
    RuleDescription description,
    Seq<IDependencyPathBasedRuleTarget> violationPath);
}
