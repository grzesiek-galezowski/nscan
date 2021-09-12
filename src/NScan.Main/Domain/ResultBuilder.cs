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

    public void AppendViolations(RuleDescription ruleDescription,
      Dictionary<RuleDescription, HashSet<string>> dictionary /* bug should be readonly */)
    {
      Result.AppendLine(ruleDescription + ": [ERROR]");
      Result.Append(string.Join(Environment.NewLine, dictionary[ruleDescription]));
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
