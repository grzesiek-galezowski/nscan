using NScan.Domain.Domain.DependencyPathBasedRules;
using NScan.Domain.Domain.NamespaceBasedRules;
using NScan.Domain.Domain.ProjectScopedRules;
using NScan.SharedKernel.Ports;

namespace NScan.Domain.Domain.Root
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