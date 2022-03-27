using NScan.SharedKernel;

namespace TddXt.NScan.Domain;

public interface IRuleReportFactory
{
  ISingleRuleReport EmptyReportFor(RuleDescription ruleDescription);
}

public class RuleReportFactory
  : IRuleReportFactory
{
  public ISingleRuleReport EmptyReportFor(RuleDescription ruleDescription)
  {
    return new SingleRuleReport(ruleDescription);
  }
}