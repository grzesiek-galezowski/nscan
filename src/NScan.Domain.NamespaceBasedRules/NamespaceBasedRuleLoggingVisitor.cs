using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public class NamespaceBasedRuleLoggingVisitor(INScanSupport support) : INamespaceBasedRuleDtoVisitor
{
  public void Visit(NoCircularUsingsRuleComplementDto dto)
  {
    support.Log(dto);
  }

  public void Visit(NoUsingsRuleComplementDto dto)
  {
    support.Log(dto);
  }
}
