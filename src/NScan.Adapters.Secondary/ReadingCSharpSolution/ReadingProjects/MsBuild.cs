using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Core.NullableReferenceTypesExtensions;

public static class MsBuild
{
  public static void ExePathAsEnvironmentVariable()
  {
    var startInfo = new ProcessStartInfo("dotnet", "--list-sdks") { RedirectStandardOutput = true };

    var process = Process.Start(startInfo).OrThrow();
    process.WaitForExit(1000);

    var output = process.StandardOutput.ReadToEnd();
    var sdkPaths = Regex.Matches(output, "([0-9]+.[0-9]+.[0-9]+) \\[(.*)\\]")
      .OfType<Match>()
      .Select(m => Path.Combine(m.Groups[2].Value, m.Groups[1].Value, "MSBuild.dll"));

    var sdkPath = sdkPaths.Last();
    Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", sdkPath, EnvironmentVariableTarget.Process);
  }
}
