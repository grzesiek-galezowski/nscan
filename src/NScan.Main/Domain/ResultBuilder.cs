using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NScan.SharedKernel;
using NullableReferenceTypesExtensions;

namespace TddXt.NScan.Domain
{
  public class ResultBuilder
  {
    public ResultBuilder()
    {
      Result = new StringBuilder();
    }

    private StringBuilder Result { get; }

    public void AppendViolations(
      RuleDescription ruleDescription, IEnumerable<RuleViolation> violations)
    {
      Result.AppendLine(ruleDescription + ": [ERROR]");
      Result.Append(string.Join(Environment.NewLine, violations.Select(v => v.ToHumanReadableString())));
    }

    public void AppendOk(RuleDescription ruleDescription)
    {
      Result.Append(ruleDescription + ": [OK]");
    }

    public void AppendRuleSeparator()
    {
      Result.Append(Environment.NewLine);
    }

    public string Text()
    {
      return Result.ToString();
    }
  }
}
