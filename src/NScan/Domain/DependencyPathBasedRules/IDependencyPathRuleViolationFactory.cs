using System.Collections.Generic;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IDependencyPathRuleViolationFactory
  {
    RuleViolation PathRuleViolation(string ruleDescription, IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
  }
}