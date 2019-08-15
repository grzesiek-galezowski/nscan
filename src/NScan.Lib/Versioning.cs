using System.Diagnostics;
using System.Reflection;

namespace TddXt.NScan.Domain.Root
{
  public static class Versioning
  {
    public static string VersionOf(Assembly assembly)
    {
      return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
    }
  }
}