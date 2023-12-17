using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public class CreateNamespaceBasedRuleVisitor(
  INamespaceBasedRuleFactory namespaceBasedRuleFactory,
  INamespacesBasedRuleSet namespacesBasedRuleSet)
  : INamespaceBasedRuleDtoVisitor
{
  public void Visit(NoCircularUsingsRuleComplementDto dto)
  {
    var rule = namespaceBasedRuleFactory.CreateNamespacesBasedRuleFrom(dto);
    namespacesBasedRuleSet.Add(rule);
  }

  public void Visit(NoUsingsRuleComplementDto dto)
  {
    var rule = namespaceBasedRuleFactory.CreateNamespacesBasedRuleFrom(dto);
    namespacesBasedRuleSet.Add(rule);
  }
}
