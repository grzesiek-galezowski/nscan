using System;
using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel;

namespace TddXt.NScan.Domain;

public class SingleRuleReport(RuleDescription ruleDescription) : ISingleRuleReport
{
  private readonly HashSet<RuleViolation> _violations = new();

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
      resultBuilder.AppendViolations(ruleDescription, ViolationsString());
    }
    else
    {
      resultBuilder.AppendOk(ruleDescription);
    }
  }

  private string ViolationsString()
  {
    return string.Join(Environment.NewLine, _violations.Select(v => v.ToHumanReadableString()));
  }
}
