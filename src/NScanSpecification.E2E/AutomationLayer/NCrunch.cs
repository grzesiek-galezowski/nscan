using System;
using AtmaFileSystem;
using NullableReferenceTypesExtensions;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScanSpecification.E2E.AutomationLayer
{
  internal static class NCrunch
  {
    public static bool RunsThisTest()
    {
      return Environment.GetEnvironmentVariable("NCrunch") == "1";
    }

    public static AbsoluteDirectoryPath OriginalSolutionPath()
    {
      return AbsoluteDirectoryPath(
        Environment.GetEnvironmentVariable("NCrunch.OriginalSolutionPath").OrThrow());
    }
  }
}
