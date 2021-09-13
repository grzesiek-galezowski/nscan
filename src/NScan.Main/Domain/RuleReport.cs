using System;
using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel;

namespace TddXt.NScan.Domain
{
  public class RuleReport : IRuleReport //bug UT (remember hashset)
  {
    private readonly HashSet<RuleViolation> _violations = new();

    public string ViolationsString()
    {
      return string.Join(Environment.NewLine, _violations.Select(v => v.ToHumanReadableString()));
    }

    public bool IsSuccessful()
    {
      return _violations.Any();
    }

    public void AddViolation(RuleViolation ruleViolation)
    {
      _violations.Add(ruleViolation);
    }

    public void AppendTo(IResultBuilder resultBuilder, RuleDescription ruleDescription)
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
  }
}
