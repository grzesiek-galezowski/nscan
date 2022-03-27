using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public interface INamespaceBasedRuleFactory
{
  INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto);
  INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoUsingsRuleComplementDto ruleDto);
}