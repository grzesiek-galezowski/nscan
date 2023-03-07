using System.Runtime.CompilerServices;
using Microsoft.Build.Locator;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

public static class MsBuild
{
#pragma warning disable CA2255
  [ModuleInitializer]
#pragma warning restore CA2255
  internal static void LoadMsBuild()
  {
    if (MSBuildLocator.CanRegister)
    {
      MSBuildLocator.RegisterDefaults();
    }
  }
}
