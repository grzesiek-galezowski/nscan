using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules
{
  public interface INamespaceBasedRuleViolationFactory
  {
    RuleViolation NoCyclesRuleViolation(string ruleDescription, string projectAssemblyName,
      IReadOnlyList<NamespaceDependencyPath> cycles);
    RuleViolation NoUsingsRuleViolation(string ruleDescription, string projectAssemblyName,
      IReadOnlyList<NamespaceDependencyPath> pathsFound);
  }
}
