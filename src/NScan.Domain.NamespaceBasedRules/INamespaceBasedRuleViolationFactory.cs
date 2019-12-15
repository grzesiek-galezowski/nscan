using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules
{
  public interface INamespaceBasedRuleViolationFactory
  {
    RuleViolation NoCyclesRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> cycles);
    RuleViolation NoUsingsRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> pathsFound);
  }
}