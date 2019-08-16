using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.Domain.DependencyPathBasedRules
{
  public interface IDependencyPathRuleViolationFactory
  {
    RuleViolation PathRuleViolation(string ruleDescription, IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
  }
}