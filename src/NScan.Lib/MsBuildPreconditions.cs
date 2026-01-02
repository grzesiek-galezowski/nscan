using System;
using System.Linq;
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
        var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
        
        if (instances.Length == 0)
        {
          throw new InvalidOperationException(
            "No MSBuild instances found. Ensure .NET SDK is installed.");
        }

        // Prioritize .NET SDK instances over Visual Studio instances for standalone tool scenarios
        // This ensures compatibility with dotnet tools and preview versions
        var selectedInstance = instances
          .OrderByDescending(i => i.DiscoveryType == DiscoveryType.DotNetSdk)
          .ThenByDescending(i => i.Version)
          .First();

        MSBuildLocator.RegisterInstance(selectedInstance);
      }
    }
#if NCRUNCH
    catch (InvalidOperationException e) when (e.Message.Contains("Microsoft.Build.Framework"))
    {
      Console.WriteLine(e.GetType());
    }
#endif
    // ReSharper disable once RedundantCatchClause
    catch
    {
      throw;
    }
  }
}
