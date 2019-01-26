using System;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  internal static class NCrunch
  {
    public static bool RunsThisTest()
    {
      return Environment.GetEnvironmentVariable("NCrunch") == "1";
    }

    public static string OriginalSolutionPath()
    {
      return Environment.GetEnvironmentVariable("NCrunch.OriginalSolutionPath");
    }
  }
}