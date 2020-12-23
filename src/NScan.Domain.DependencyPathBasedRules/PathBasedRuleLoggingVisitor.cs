using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.DependencyPathBasedRules
{
  public class PathBasedRuleLoggingVisitor : IPathBasedRuleDtoVisitor
  {
    private readonly INScanSupport _support;

    public PathBasedRuleLoggingVisitor(INScanSupport support)
    {
      _support = support;
    }

    public void Visit(IndependentRuleComplementDto dto)
    {
      _support.Log(dto);
    }
  }
}
