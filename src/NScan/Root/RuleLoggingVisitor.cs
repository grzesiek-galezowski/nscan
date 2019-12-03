using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public class RuleLoggingVisitor : IRuleDtoVisitor, IPathBasedRuleDtoVisitor, INamespaceBasedRuleDtoVisitor, IProjectScopedRuleDtoVisitor
  {
    private readonly INScanSupport _support;

    public RuleLoggingVisitor(INScanSupport support)
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

    public void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      _support.Log(dto);
    }

    public void Visit(NoUsingsRuleComplementDto dto)
    {
      _support.Log(dto);
    }

    public void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      _support.Log(dto);
    }

    public void Visit(IndependentRuleComplementDto dto)
    {
      _support.Log(dto);
    }
  }
}