using System;
using AtmaFileSystem;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  internal static class NCrunch
  {
    public static bool RunsThisTest()
    {
      return Environment.GetEnvironmentVariable("NCrunch") == "1";
    }

    public static AbsoluteDirectoryPath OriginalSolutionPath()
    {
      return AbsoluteDirectoryPath.Value(Environment.GetEnvironmentVariable("NCrunch.OriginalSolutionPath"));
    }
  }
}