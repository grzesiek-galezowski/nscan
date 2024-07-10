using System;
using Microsoft.Build.Locator;

namespace NScan.Lib;

public static class MsBuildPreconditions
{
  public static void RegisterMsBuild()
  {
    if (!MSBuildLocator.IsRegistered)
    {
      MSBuildLocator.RegisterDefaults();
    }
  }
}
