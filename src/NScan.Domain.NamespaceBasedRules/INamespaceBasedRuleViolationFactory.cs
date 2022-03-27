using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public interface INamespaceBasedRuleViolationFactory
{
  RuleViolation NoCyclesRuleViolation(
    RuleDescription description,
    AssemblyName projectAssemblyName,
    IReadOnlyList<NamespaceDependencyPath> cycles);
  RuleViolation NoUsingsRuleViolation(
    RuleDescription description,
    AssemblyName projectAssemblyName,
    IReadOnlyList<NamespaceDependencyPath> pathsFound);
}