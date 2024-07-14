using System;
using Microsoft.Build.Locator;

namespace NScan.Lib;

public static class MsBuildPreconditions
{
  public static void RegisterMsBuild()
  {
    try
    {
      if (!MSBuildLocator.IsRegistered)
      {
        MSBuildLocator.RegisterDefaults();
      }
    }
    #if NCRUNCH
    catch (System.InvalidOperationException e) when (e.Message.Contains("Microsoft.Build.Framework"))
    {
      Console.WriteLine(e.GetType());
    }
    #endif
    finally
    {

    }
  }
}
