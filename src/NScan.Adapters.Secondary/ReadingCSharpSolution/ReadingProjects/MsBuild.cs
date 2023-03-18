using System.Runtime.CompilerServices;
using NScan.Lib;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

public static class MsBuild
{
#pragma warning disable CA2255
  [ModuleInitializer]
#pragma warning restore CA2255
  internal static void LoadMsBuild()
  {
    MsBuildPreconditions.RegisterMsBuild();
  }
}
