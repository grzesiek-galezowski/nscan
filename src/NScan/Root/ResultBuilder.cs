using System;
using System.Collections.Generic;
using System.Text;

namespace NScan.Domain.Root
{
  public class ResultBuilder
  {
    public ResultBuilder()
    {
      Result = new StringBuilder();
    }

    private StringBuilder Result { get; }

    public void AppendViolations(string ruleDescription, Dictionary<string, HashSet<string>> dictionary)
    {
      Result.AppendLine(ruleDescription + ": [ERROR]");
      Result.Append(string.Join(Environment.NewLine, dictionary[ruleDescription]));
    }

    public void AppendOk(string ruleDescription)
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