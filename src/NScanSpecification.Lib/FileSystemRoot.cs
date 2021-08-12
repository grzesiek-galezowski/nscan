using System.Runtime.InteropServices;

namespace NScanSpecification.Lib
{
  public static class FileSystemRoot
  {
    public static string PlatformSpecificValue()
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        return "C:";
      }
      else
      {
        return "/Root";
      }
    }
  }
}
