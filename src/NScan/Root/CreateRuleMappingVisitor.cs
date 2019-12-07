using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public class CreateRuleMappingVisitor : IRuleDtoVisitor
  {
    private readonly INamespaceBasedRuleFactory _namespaceBasedRuleFactory;
    private readonly IProjectScopedRuleFactory _projectScopedRuleFactory;
    private readonly IDependencyBasedRuleFactory _dependencyBasedRuleFactory;
    private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;
    private readonly IProjectScopedRuleSet _projectScopedRules;
    private readonly IPathRuleSet _pathRules;

    public CreateRuleMappingVisitor(
      IRuleFactory ruleFactory, 
      INamespacesBasedRuleSet namespacesBasedRuleSet, 
      IProjectScopedRuleSet projectScopedRules, 
      IPathRuleSet pathRules)
    {
      _namespaceBasedRuleFactory = (INamespaceBasedRuleFactory)ruleFactory;
      _dependencyBasedRuleFactory = (IDependencyBasedRuleFactory)ruleFactory;
      _projectScopedRuleFactory = (IProjectScopedRuleFactory)ruleFactory;
      _namespacesBasedRuleSet = namespacesBasedRuleSet;
      _projectScopedRules = projectScopedRules;
      _pathRules = pathRules;
    }

    public void Visit(HasTargetFrameworkRuleComplementDto arg)
    {
      var rule = _projectScopedRuleFactory.CreateProjectScopedRuleFrom(arg);
      _projectScopedRules.Add(rule);
    }

    public void Visit(HasAttributesOnRuleComplementDto dto)
    {
      var rule = _projectScopedRuleFactory.CreateProjectScopedRuleFrom(dto);
      _projectScopedRules.Add(rule);
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

    public void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      var rule = _projectScopedRuleFactory.CreateProjectScopedRuleFrom(dto);
      _projectScopedRules.Add(rule);
    }

    public void Visit(IndependentRuleComplementDto dto)
    {
      var rule = _dependencyBasedRuleFactory.CreateDependencyRuleFrom(dto);
      _pathRules.Add(rule);
    }
  }
}