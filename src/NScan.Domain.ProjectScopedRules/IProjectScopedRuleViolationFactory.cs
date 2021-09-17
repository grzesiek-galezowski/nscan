using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface IProjectScopedRuleViolationFactory
  {
    RuleViolation ProjectScopedRuleViolation(RuleDescription description, string violationDescription);
  }
}
