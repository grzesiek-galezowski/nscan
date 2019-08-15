using System.Collections.Generic;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public interface IDependencyPathRuleViolationFactory
  {
    RuleViolation PathRuleViolation(string ruleDescription, IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
  }
}