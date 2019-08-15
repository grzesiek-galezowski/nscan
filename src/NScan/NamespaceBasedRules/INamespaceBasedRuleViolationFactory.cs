using System.Collections.Generic;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.NamespaceBasedRules
{
  public interface INamespaceBasedRuleViolationFactory
  {
    RuleViolation NoCyclesRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> cycles);
  }
}