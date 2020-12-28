using System;
using System.Collections.Generic;
using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public class NamespaceBasedReportFragmentsFormat : INamespaceBasedReportFragmentsFormat
  {
    public string ApplyTo(IReadOnlyList<NamespaceDependencyPath> paths, string header)
    {
      string result = string.Empty;
      for (var pathIndex = 0; pathIndex < paths.Count; pathIndex++)
      {
        result += $"{header} {pathIndex + 1}:{Environment.NewLine}";
        var singlePath = paths[pathIndex];
        for (var cycleElementIndex = 0; cycleElementIndex < singlePath.AsList().Count; cycleElementIndex++)
        {
          var segment = singlePath.AsList()[cycleElementIndex].Value;
          result += Indent(cycleElementIndex) + segment + Environment.NewLine;
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
