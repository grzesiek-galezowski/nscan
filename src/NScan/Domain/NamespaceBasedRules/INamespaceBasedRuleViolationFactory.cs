using System.Collections.Generic;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.NamespaceBasedRules
{
  public interface INamespaceBasedRuleViolationFactory
  {
    RuleViolation NoCyclesRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> cycles);
  }
}