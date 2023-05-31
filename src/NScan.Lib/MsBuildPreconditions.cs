using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using Core.NullableReferenceTypesExtensions;

namespace NScan.Lib;

public static class MsBuildPreconditions
{
  public static void RegisterMsBuild()
  {
    var output = GetDotnetSdkListOutput();
    var sdkPath = GetCompatibleSdkPath(output);
    Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", sdkPath, EnvironmentVariableTarget.Process);
  }

  private static string GetCompatibleSdkPath(string output)
  {
    var sdkPaths = Regex.Matches(output, "([0-9]+.[0-9]+.[0-9]+) \\[(.*)\\]")
      .OfType<Match>()
      .Where(IsForCurrentlyRunningNetVersion)
      .Select(m => Path.Combine(m.Groups[2].Value, SdkVersionIn(m), "MSBuild.dll"));

    var sdkPath = sdkPaths.Last();
    return sdkPath;
  }

  private static string GetDotnetSdkListOutput()
  {
    var startInfo = new ProcessStartInfo("dotnet", "--list-sdks") { RedirectStandardOutput = true };
    var process = Process.Start(startInfo).OrThrow();
    process.WaitForExit(1000);
    var output = process.StandardOutput.ReadToEnd();
    return output;
  }

  private static bool IsForCurrentlyRunningNetVersion(Match m)
  {
    var firstVersionNum = GetCurrentlyExecutingDotnetVersionNumber();
    return SdkVersionIn(m).StartsWith(firstVersionNum);
  }

  private static string GetCurrentlyExecutingDotnetVersionNumber()
  {
    var targetFrameworkAttribute = (TargetFrameworkAttribute)
      Assembly.GetExecutingAssembly()
        .GetCustomAttributes(typeof(TargetFrameworkAttribute), false)
        .Single();

    var firstVersionNum = targetFrameworkAttribute.FrameworkDisplayName.OrThrow()[5..];
    return firstVersionNum;
  }

  private static string SdkVersionIn(Match m)
  {
    return m.Groups[1].Value;
  }
}
