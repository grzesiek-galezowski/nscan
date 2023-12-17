using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.DependencyPathBasedRules;

public class PathBasedRuleLoggingVisitor(INScanSupport support) : IPathBasedRuleDtoVisitor
{
  public void Visit(IndependentRuleComplementDto dto)
  {
    support.Log(dto);
  }
}
