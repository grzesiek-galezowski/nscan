using System.IO;

static internal class TemporaryDirectory
{
  public static DirectoryInfo CreateNew()
  {
    var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    Directory.CreateDirectory(tempDirectory);
    return new DirectoryInfo(tempDirectory);
  }
}