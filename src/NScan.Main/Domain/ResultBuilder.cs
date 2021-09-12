using System;
using System.Collections.Generic;
using System.Text;
using NScan.SharedKernel;

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
      RuleDescription ruleDescription,
      IReadOnlyDictionary<RuleDescription, HashSet<string>> violation)
    {
      Result.AppendLine(ruleDescription + ": [ERROR]");
      Result.Append(string.Join(Environment.NewLine, violation[ruleDescription]));
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
