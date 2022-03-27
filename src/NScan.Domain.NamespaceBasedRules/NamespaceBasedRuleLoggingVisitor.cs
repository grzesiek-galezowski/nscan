using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public class NamespaceBasedRuleLoggingVisitor : INamespaceBasedRuleDtoVisitor
{
  private readonly INScanSupport _support;

  public NamespaceBasedRuleLoggingVisitor(INScanSupport support)
  {
    _support = support;
  }

  public void Visit(NoCircularUsingsRuleComplementDto dto)
  {
    _support.Log(dto);
  }

  public void Visit(NoUsingsRuleComplementDto dto)
  {
    _support.Log(dto);
  }
}