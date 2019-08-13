using System.Collections.Generic;
using NScan.SharedKernel.SharedKernel;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IDependencyPathRuleViolationFactory
  {
    RuleViolation PathRuleViolation(string ruleDescription, IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
  }
}