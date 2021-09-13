using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NScan.SharedKernel;
using NullableReferenceTypesExtensions;

namespace TddXt.NScan.Domain
{
  public class ResultBuilder : IResultBuilder
  {
    public ResultBuilder()
    {
      Result = new StringBuilder();
    }

    private StringBuilder Result { get; }

    public void AppendViolations(
      RuleDescription ruleDescription, string violationsString)
    {
      Result.AppendLine(ruleDescription + ": [ERROR]");
      Result.Append(violationsString);
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
