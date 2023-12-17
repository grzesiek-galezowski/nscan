using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules;

public class CreateProjectScopedRuleVisitor(
  IProjectScopedRuleFactory projectScopedRuleFactory,
  IProjectScopedRuleSet projectScopedRules)
  : IProjectScopedRuleDtoVisitor
{
  public void Visit(HasTargetFrameworkRuleComplementDto arg)
  {
    var rule = projectScopedRuleFactory.CreateProjectScopedRuleFrom(arg);
    projectScopedRules.Add(rule);
  }

  public void Visit(HasPropertyRuleComplementDto dto)
  {
    var rule = projectScopedRuleFactory.CreateProjectScopedRuleFrom(dto);
    projectScopedRules.Add(rule);
  }

  public void Visit(HasAttributesOnRuleComplementDto dto)
  {
    var rule = projectScopedRuleFactory.CreateProjectScopedRuleFrom(dto);
    projectScopedRules.Add(rule);
  }

  public void Visit(CorrectNamespacesRuleComplementDto dto)
  {
    var rule = projectScopedRuleFactory.CreateProjectScopedRuleFrom(dto);
    projectScopedRules.Add(rule);
  }
}
