using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules
{
  public interface INamespaceBasedRuleViolationFactory
  {
    RuleViolation NoCyclesRuleViolation(
      RuleDescription description, 
      string projectAssemblyName,
      IReadOnlyList<NamespaceDependencyPath> cycles);
    RuleViolation NoUsingsRuleViolation(
      RuleDescription description, 
      string projectAssemblyName,
      IReadOnlyList<NamespaceDependencyPath> pathsFound);
  }
}
