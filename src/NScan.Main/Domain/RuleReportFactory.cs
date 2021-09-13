using NScan.SharedKernel;

namespace TddXt.NScan.Domain
{
  public interface IRuleReportFactory
  {
    IRuleReport EmptyRuleReport();
  }

  public class RuleReportFactory
    : IRuleReportFactory
  {
    public IRuleReport EmptyRuleReport()
    {
      return new RuleReport();
    }
  }
}
