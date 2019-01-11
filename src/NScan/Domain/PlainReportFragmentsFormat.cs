using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace TddXt.NScan.Domain
{
  public class PlainReportFragmentsFormat : IReportFragmentsFormat
  {
    public string ApplyToPath(IReadOnlyList<IReferencedProject> violationPath)
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
        result += $"Cycle {i + 1}:{NewLine}";
        var singleCycle = cycles[i];
        for (var j = 0; j < singleCycle.Count; j++)
        {
          var segment = singleCycle[j];
          result += Indent(j) + segment + NewLine;
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