using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules
{
  public class CreateNamespaceBasedRuleVisitor : INamespaceBasedRuleDtoVisitor
  {
    private readonly INamespaceBasedRuleFactory _namespaceBasedRuleFactory;
    private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;

    public CreateNamespaceBasedRuleVisitor(
      INamespaceBasedRuleFactory namespaceBasedRuleFactory,
      INamespacesBasedRuleSet namespacesBasedRuleSet)
    {
      _namespaceBasedRuleFactory = namespaceBasedRuleFactory;
      _namespacesBasedRuleSet = namespacesBasedRuleSet;
    }

    public void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      var rule = _namespaceBasedRuleFactory.CreateNamespacesBasedRuleFrom(dto);
      _namespacesBasedRuleSet.Add(rule);
    }

    public void Visit(NoUsingsRuleComplementDto dto)
    {
      var rule = _namespaceBasedRuleFactory.CreateNamespacesBasedRuleFrom(dto);
      _namespacesBasedRuleSet.Add(rule);
    }
  }
}