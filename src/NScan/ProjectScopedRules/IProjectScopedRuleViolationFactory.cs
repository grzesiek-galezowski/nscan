using NScan.SharedKernel;

namespace NScan.Domain.ProjectScopedRules
{
  public interface IProjectScopedRuleViolationFactory
  {
    RuleViolation ProjectScopedRuleViolation(string ruleDescription, string violationDescription);
  }
}