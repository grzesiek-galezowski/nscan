using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public class NamespaceBasedRuleFactory(INamespaceBasedRuleViolationFactory ruleViolationFactory)
  : INamespaceBasedRuleFactory
{
  public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto)
  {
    return new NoCircularUsingsRule(ruleDto, ruleViolationFactory);
  }

  public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoUsingsRuleComplementDto ruleDto)
  {
    return new NoUsingsRule(ruleDto, ruleViolationFactory);
  }
}
