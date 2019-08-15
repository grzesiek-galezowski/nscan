using System.Diagnostics;
using System.Reflection;

namespace NScan.Lib
{
  public static class Versioning
  {
    public static string VersionOf(Assembly assembly)
    {
      return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
    }
  }
}