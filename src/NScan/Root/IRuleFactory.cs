using NScan.Domain.DependencyPathBasedRules;
using NScan.Domain.NamespaceBasedRules;
using NScan.Domain.ProjectScopedRules;
using NScan.SharedKernel.RuleDtos;

namespace NScan.Domain.Root
{
  public interface IRuleFactory
  {
    IDependencyRule CreateDependencyRuleFrom(IndependentRuleComplementDto independentRuleComplementDto);
    IProjectScopedRule CreateProjectScopedRuleFrom(CorrectNamespacesRuleComplementDto ruleDto);
    INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto);
    IProjectScopedRule CreateProjectScopedRuleFrom(HasAttributesOnRuleComplementDto ruleDto);
    IProjectScopedRule CreateProjectScopedRuleFrom(HasTargetFrameworkRuleComplementDto ruleDto);
  }
}