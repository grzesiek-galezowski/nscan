using System.IO;

namespace NScanSpecification.E2E.AutomationLayer;

public static class RelevantPaths
{
  public static DirectoryInfo CreateRandomDirectory()
  {
    return CreateRandomDirectory(string.Empty);
  }

  public static DirectoryInfo CreateRandomDirectory(string prefix)
  {
    var tempDirectory = Path.Combine(Path.GetTempPath(), prefix + Path.GetRandomFileName());
    Directory.CreateDirectory(tempDirectory);
    return new DirectoryInfo(tempDirectory);
  }

  public static SolutionDir CreateHomeForFixtureSolution(string solutionName)
  {
    return new SolutionDir(CreateRandomDirectory(), solutionName);
  }
}