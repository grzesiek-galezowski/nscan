using System;
using NScan.Lib;

namespace NScanSpecification.Lib.AutomationLayer;

public static class PathFormat
{
  public static string For(string header, string[] cyclePath)
  {
    var result = $"{header}:{Environment.NewLine}";
    for (var i = 0; i < cyclePath.Length; ++i)
    {
      result += i.Indentations() + cyclePath[i] + Environment.NewLine;
    }

    return result;
  }
}