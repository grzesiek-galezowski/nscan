using NScan.NamespaceBasedRules;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.Domain.Root
{
  public interface INamespaceBasedRuleFactory
  {
    INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto);
    INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoUsingsRuleComplementDto ruleDto);
  }
}