using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public interface IProjectScopedRuleViolationFactory
  {
    RuleViolation ProjectScopedRuleViolation(string ruleDescription, string violationDescription);
  }
}