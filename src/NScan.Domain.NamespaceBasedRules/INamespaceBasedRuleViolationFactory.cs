using LanguageExt;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public interface INamespaceBasedRuleViolationFactory
{
  RuleViolation NoCyclesRuleViolation(
    RuleDescription description,
    AssemblyName projectAssemblyName,
    Seq<NamespaceDependencyPath> cycles);
  RuleViolation NoUsingsRuleViolation(
    RuleDescription description,
    AssemblyName projectAssemblyName,
    Seq<NamespaceDependencyPath> pathsFound);
}
