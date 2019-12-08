using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules
{
  public class CreateProjectScopedRuleVisitor : IProjectScopedRuleDtoVisitor
  {
    private readonly IProjectScopedRuleFactory _projectScopedRuleFactory;
    private readonly IProjectScopedRuleSet _projectScopedRules;

    public CreateProjectScopedRuleVisitor(IProjectScopedRuleFactory projectScopedRuleFactory,
      IProjectScopedRuleSet projectScopedRules)
    {
      _projectScopedRuleFactory = projectScopedRuleFactory;
      _projectScopedRules = projectScopedRules;
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

    public void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      var rule = _projectScopedRuleFactory.CreateProjectScopedRuleFrom(dto);
      _projectScopedRules.Add(rule);
    }
  }
}