using NScan.ProjectScopedRules;
using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public class ProjectScopedRuleViolationFactory : IProjectScopedRuleViolationFactory
  {
    public RuleViolation ProjectScopedRuleViolation(string ruleDescription, string violationDescription)
    {
      return new RuleViolation(ruleDescription, string.Empty, violationDescription);
    }
  }
}