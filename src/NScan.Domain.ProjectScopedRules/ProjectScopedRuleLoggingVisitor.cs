using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules
{
  public class ProjectScopedRuleLoggingVisitor : IProjectScopedRuleDtoVisitor
  {
    private readonly INScanSupport _support;

    public ProjectScopedRuleLoggingVisitor(INScanSupport support)
    {
      _support = support;
    }

    public void Visit(HasTargetFrameworkRuleComplementDto dto)
    {
      _support.Log(dto);
    }

    public void Visit(HasAttributesOnRuleComplementDto dto)
    {
      _support.Log(dto);
    }

    public void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      _support.Log(dto);
    }
  }
}
