using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public interface IDependencyBasedRuleFactory
  {
    IDependencyRule CreateDependencyRuleFrom(IndependentRuleComplementDto independentRuleComplementDto);
  }

  public interface IProjectScopedRuleFactory
  {
    IProjectScopedRule CreateProjectScopedRuleFrom(CorrectNamespacesRuleComplementDto ruleDto);
    IProjectScopedRule CreateProjectScopedRuleFrom(HasAttributesOnRuleComplementDto ruleDto);
    IProjectScopedRule CreateProjectScopedRuleFrom(HasTargetFrameworkRuleComplementDto ruleDto);
  }

  public interface INamespaceBasedRuleFactory
  {
    INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto);
    INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoUsingsRuleComplementDto ruleDto);
  }

  public interface IRuleFactory : IDependencyBasedRuleFactory, IProjectScopedRuleFactory, INamespaceBasedRuleFactory
  {
  }
}