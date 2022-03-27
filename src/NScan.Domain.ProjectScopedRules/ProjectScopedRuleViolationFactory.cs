using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class ProjectScopedRuleViolationFactory : IProjectScopedRuleViolationFactory
{
  public RuleViolation ProjectScopedRuleViolation(
    RuleDescription description, 
    string violationDescription)
  {
    return RuleViolation.Create(description, string.Empty, violationDescription);
  }
}