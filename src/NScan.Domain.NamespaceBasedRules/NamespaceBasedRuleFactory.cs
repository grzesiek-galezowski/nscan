using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public class NamespaceBasedRuleFactory : INamespaceBasedRuleFactory
{
  private readonly INamespaceBasedRuleViolationFactory _ruleViolationFactory;

  public NamespaceBasedRuleFactory(INamespaceBasedRuleViolationFactory ruleViolationFactory)
  {
    _ruleViolationFactory = ruleViolationFactory;
  }

  public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto)
  {
    return new NoCircularUsingsRule(ruleDto, _ruleViolationFactory);
  }

  public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoUsingsRuleComplementDto ruleDto)
  {
    return new NoUsingsRule(ruleDto, _ruleViolationFactory);
  }
}