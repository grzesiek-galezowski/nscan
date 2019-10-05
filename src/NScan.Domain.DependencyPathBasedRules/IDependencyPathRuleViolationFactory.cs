using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules
{
  public interface IDependencyPathRuleViolationFactory
  {
    RuleViolation PathRuleViolation(string ruleDescription, IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
  }
}