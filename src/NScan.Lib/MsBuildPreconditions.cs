using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Locator;

namespace NScan.Lib;

public static class MsBuildPreconditions
{
  public static void RegisterMsBuild()
  {
    Environment.SetEnvironmentVariable("MSBuildEnableWorkloadResolver", "false", EnvironmentVariableTarget.Process);
    if (MSBuildLocator.CanRegister)
    {
      MSBuildLocator.RegisterDefaults();
    }
  }
}
