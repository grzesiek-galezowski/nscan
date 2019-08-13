using System;
using System.Collections.Generic;
using System.Linq;
using NScan.Lib;
using TddXt.NScan.Domain.DependencyPathBasedRules;

namespace TddXt.NScan.Domain.Root
{
  public class PlainReportFragmentsFormat : IReportFragmentsFormat
  {
    public string ApplyToPath(IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath)
    {
      return violationPath.Skip(1).Aggregate(
        "[" + violationPath.First().ToString() + "]",
        (total, current) => total + "->" + "[" + current.ToString() + "]");
    }

    public string ApplyToCycles(IReadOnlyList<IReadOnlyList<string>> cycles)
    {
      string result = string.Empty;
      for (var i = 0; i < cycles.Count; i++)
      {
        result += $"Cycle {i + 1}:{Environment.NewLine}";
        var singleCycle = cycles[i];
        for (var j = 0; j < singleCycle.Count; j++)
        {
          var segment = singleCycle[j];
          result += Indent(j) + segment + Environment.NewLine;
        }
      }
      return result;
    }

    private static string Indent(int j)
    {
      return ((j+1)*2).Spaces();
    }
  }
}