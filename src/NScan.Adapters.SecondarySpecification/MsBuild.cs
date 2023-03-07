using System.Runtime.CompilerServices;
using Microsoft.Build.Locator;

namespace NScan.Adapters.SecondarySpecification;

public static class MsBuild
{
  [ModuleInitializer]
  internal static void LoadMsBuild()
  {
    if (MSBuildLocator.CanRegister)
    {
      MSBuildLocator.RegisterDefaults();
    }
  }
}
