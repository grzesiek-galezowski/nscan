using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel;

namespace TddXt.NScan.Domain;

public class AnalysisReportInProgress(IRuleReportFactory ruleReportFactory) : IAnalysisReportInProgress
{
  private readonly Dictionary<RuleDescription, ISingleRuleReport> _reportsByRule = new();

  public void PutContentInto(IResultBuilder resultBuilder)
  {
    foreach (var singleRuleReport in _reportsByRule.Values)
    {
      singleRuleReport.AppendTo(resultBuilder);

      if (!singleRuleReport.Equals(_reportsByRule.Values.Last()))
      {
        resultBuilder.AppendRuleSeparator();
      }
    }
  }

  public void StartedCheckingTarget(AssemblyName assemblyName)
  {
    //bug throw new System.NotImplementedException();
  }

  public void FinishedEvaluatingRule(RuleDescription ruleDescription)
  {
    AddRuleIfNotRegisteredYet(ruleDescription);
  }

  public bool IsFailure()
  {
    return _reportsByRule.Values.Any(ruleReport => ruleReport.IsFailed());
  }

  public void Add(RuleViolation ruleViolation)
  {
    InitializeForCollecting(ruleViolation.RuleDescription);
    _reportsByRule[ruleViolation.RuleDescription].Add(ruleViolation);
  }

  private void InitializeForCollecting(RuleDescription ruleName)
  {
    AddRuleIfNotRegisteredYet(ruleName);
  }

  private void AddRuleIfNotRegisteredYet(RuleDescription ruleName)
  {
    if (!_reportsByRule.Keys.Contains(ruleName))
    {
      _reportsByRule[ruleName] = ruleReportFactory.EmptyReportFor(ruleName);
    }
  }
}
