using System;
using System.Collections.Generic;
using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public class NamespaceBasedReportFragmentsFormat : INamespaceBasedReportFragmentsFormat, INamespaceDependencyPathFormat
  {
    public string ApplyTo(IReadOnlyList<NamespaceDependencyPath> paths, string header)
    {
      string result = string.Empty;
      for (var pathIndex = 0; pathIndex < paths.Count; pathIndex++)
      {
        result += $"{header} {pathIndex + 1}:{Environment.NewLine}";
        result += paths[pathIndex].ToStringFormatted(this);
      }
      return result;
    }

    public string ElementTerminator()
    {
      return Environment.NewLine;
    }

    public string ElementIndentation(int elementIndex)
    {
      return ((elementIndex+1)*2).Spaces();
    }
  }
}
