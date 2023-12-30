using System.Collections.Generic;
using LanguageExt;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules;

public interface IDependencyPathRuleViolationFactory
{
  RuleViolation PathRuleViolation(
    RuleDescription description,
    Seq<IDependencyPathBasedRuleTarget> violationPath);
}
