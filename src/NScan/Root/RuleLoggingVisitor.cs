using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos;

namespace NScan.Domain.Root
{
  public class RuleLoggingVisitor : IRuleDtoVisitor
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