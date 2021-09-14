using System;
using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel;

namespace TddXt.NScan.Domain
{
  public class SingleRuleReport : ISingleRuleReport
  {
    private readonly RuleDescription _ruleDescription;
    private readonly HashSet<RuleViolation> _violations = new();

    public SingleRuleReport(RuleDescription ruleDescription)
    {
      _ruleDescription = ruleDescription;
    }

    public bool IsFailed()
    {
      return _violations.Any();
    }

    public void Add(RuleViolation ruleViolation)
    {
      _violations.Add(ruleViolation);
    }

    public void AppendTo(IResultBuilder resultBuilder)
    {
      if (_violations.Any())
      {
        resultBuilder.AppendViolations(_ruleDescription, ViolationsString());
      }
      else
      {
        resultBuilder.AppendOk(_ruleDescription);
      }
    }

    private string ViolationsString()
    {
      return string.Join(Environment.NewLine, _violations.Select(v => v.ToHumanReadableString()));
    }
  }
}
