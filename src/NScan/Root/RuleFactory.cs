using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public class RuleFactory : IRuleFactory, IDependencyBasedRuleFactory, IProjectScopedRuleFactory, INamespaceBasedRuleFactory
  {
    private readonly IRuleViolationFactory _ruleViolationFactory;
    private readonly IDependencyBasedRuleFactory _dependencyPathRuleFactory;
    private readonly ProjectScopedRuleFactory _projectScopedRuleFactory;
    private readonly NamespaceBasedRuleFactory _namespaceBasedRuleFactory;

    public RuleFactory(IRuleViolationFactory ruleViolationFactory)
    {
      _ruleViolationFactory = ruleViolationFactory;
      _dependencyPathRuleFactory = new DependencyPathRuleFactory(ruleViolationFactory);
      _projectScopedRuleFactory = new ProjectScopedRuleFactory(ruleViolationFactory);
      _namespaceBasedRuleFactory = new NamespaceBasedRuleFactory(ruleViolationFactory);
    }

    public IDependencyRule CreateDependencyRuleFrom(IndependentRuleComplementDto independentRuleComplementDto)
    {
      return _dependencyPathRuleFactory.CreateDependencyRuleFrom(independentRuleComplementDto);
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(CorrectNamespacesRuleComplementDto ruleDto)
    {
      return _projectScopedRuleFactory.CreateProjectScopedRuleFrom(ruleDto);
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(HasAttributesOnRuleComplementDto ruleDto)
    {
      return _projectScopedRuleFactory.CreateProjectScopedRuleFrom(ruleDto);
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(HasTargetFrameworkRuleComplementDto ruleDto)
    {
      return _projectScopedRuleFactory.CreateProjectScopedRuleFrom(ruleDto);
    }

    public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto)
    {
      return _namespaceBasedRuleFactory.CreateNamespacesBasedRuleFrom(ruleDto);
    }

    public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoUsingsRuleComplementDto ruleDto)
    {
      return _namespaceBasedRuleFactory.CreateNamespacesBasedRuleFrom(ruleDto);
    }
  }


}
