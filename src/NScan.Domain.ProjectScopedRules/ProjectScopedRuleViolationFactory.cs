using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public class ProjectScopedRuleViolationFactory : IProjectScopedRuleViolationFactory
  {
    public RuleViolation ProjectScopedRuleViolation(string ruleDescription, string violationDescription)
    {
      return new RuleViolation(ruleDescription, string.Empty, violationDescription);
    }
  }
}