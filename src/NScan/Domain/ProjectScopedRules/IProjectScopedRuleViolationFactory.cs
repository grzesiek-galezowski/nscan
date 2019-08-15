using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.ProjectScopedRules
{
  public interface IProjectScopedRuleViolationFactory
  {
    RuleViolation ProjectScopedRuleViolation(string ruleDescription, string violationDescription);
  }
}