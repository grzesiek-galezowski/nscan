using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel.RuleDtos;

namespace NScan.Domain.Root
{
  public class CreateRuleMappingVisitor : IRuleDtoVisitor
  {
    private readonly IRuleFactory _ruleFactory;
    private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;
    private readonly IProjectScopedRuleSet _projectScopedRules;
    private readonly IPathRuleSet _pathRules;

    public CreateRuleMappingVisitor(
      IRuleFactory ruleFactory, 
      INamespacesBasedRuleSet namespacesBasedRuleSet, 
      IProjectScopedRuleSet projectScopedRules, 
      IPathRuleSet pathRules)
    {
      _ruleFactory = ruleFactory;
      _namespacesBasedRuleSet = namespacesBasedRuleSet;
      _projectScopedRules = projectScopedRules;
      _pathRules = pathRules;
    }

    public void Visit(HasTargetFrameworkRuleComplementDto arg)
    {
      var rule = _ruleFactory.CreateProjectScopedRuleFrom(arg);
      _projectScopedRules.Add(rule);
    }

    public void Visit(HasAttributesOnRuleComplementDto dto)
    {
      var rule = _ruleFactory.CreateProjectScopedRuleFrom(dto);
      _projectScopedRules.Add(rule);
    }

    public void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      var rule = _ruleFactory.CreateNamespacesBasedRuleFrom(dto);
      _namespacesBasedRuleSet.Add(rule);
    }

    public void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      var rule = _ruleFactory.CreateProjectScopedRuleFrom(dto);
      _projectScopedRules.Add(rule);
    }

    public void Visit(IndependentRuleComplementDto dto)
    {
      var rule = _ruleFactory.CreateDependencyRuleFrom(dto);
      _pathRules.Add(rule);
    }
  }
}