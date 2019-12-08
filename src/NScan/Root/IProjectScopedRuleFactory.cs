using NScan.ProjectScopedRules;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public interface IProjectScopedRuleFactory
  {
    IProjectScopedRule CreateProjectScopedRuleFrom(CorrectNamespacesRuleComplementDto ruleDto);
    IProjectScopedRule CreateProjectScopedRuleFrom(HasAttributesOnRuleComplementDto ruleDto);
    IProjectScopedRule CreateProjectScopedRuleFrom(HasTargetFrameworkRuleComplementDto ruleDto);
  }
}