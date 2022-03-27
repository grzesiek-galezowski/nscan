using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules;

public interface IDependencyPathRuleViolationFactory
{
  RuleViolation PathRuleViolation(
    RuleDescription description,
    IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
}