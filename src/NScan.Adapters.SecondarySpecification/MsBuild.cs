using System.Runtime.CompilerServices;
using NScan.Lib;

namespace NScan.Adapters.SecondarySpecification;

public static class MsBuild
{
  [ModuleInitializer]
  internal static void LoadMsBuild()
  {
    MsBuildPreconditions.RegisterMsBuild();
  }
}
