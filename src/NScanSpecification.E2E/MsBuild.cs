using System.Runtime.CompilerServices;
using NScan.Lib;

namespace NScanSpecification.E2E;

public static class MsBuild
{
  [ModuleInitializer]
  internal static void LoadMsBuild()
  {
    MsBuildPreconditions.RegisterMsBuild();
  }
}
