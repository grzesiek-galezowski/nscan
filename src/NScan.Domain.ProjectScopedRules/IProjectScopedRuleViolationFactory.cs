using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface IProjectScopedRuleViolationFactory
  {
    RuleViolation ProjectScopedRuleViolation(string ruleDescription, string violationDescription);
  }
}