using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules;

public class ProjectScopedRuleLoggingVisitor(INScanSupport support) : IProjectScopedRuleDtoVisitor
{
  public void Visit(HasTargetFrameworkRuleComplementDto dto)
  {
    support.Log(dto);
  }

  public void Visit(HasPropertyRuleComplementDto dto)
  {
    support.Log(dto);
  }

  public void Visit(HasAttributesOnRuleComplementDto dto)
  {
    support.Log(dto);
  }

  public void Visit(CorrectNamespacesRuleComplementDto dto)
  {
    support.Log(dto);
  }
}
