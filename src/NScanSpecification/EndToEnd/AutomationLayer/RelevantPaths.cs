using System.IO;
using System.Linq;
using System.Reflection;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public static class RelevantPaths
  {
    public static DirectoryInfo CreateRandomPath()
    {
      var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(tempDirectory);
      return new DirectoryInfo(tempDirectory);
    }

    public static string RepositoryPath()
    {
      if (NCrunch.RunsThisTest())
      {
        var originalSolutionPath = NCrunch.OriginalSolutionPath();
        return Path.Combine(originalSolutionPath.Split("nscan").First(), "nscan");
      }
      else
      {
        var executingAssemblyPath = new FileInfo(
          Assembly.GetExecutingAssembly().EscapedCodeBase.Split("file:///").ToArray()[1]).Directory;
        while (!Directory.EnumerateDirectories(executingAssemblyPath.FullName).Any(s => s.EndsWith(".git")))
        {
          executingAssemblyPath = executingAssemblyPath.Parent;
        }

        return executingAssemblyPath.FullName;
      }

    }

    public static string NscanConsoleProjectPath(string repositoryPath)
    {
      return Path.Combine(
        repositoryPath, "src", "NScan.Console", "NScan.Console.csproj");
    }
  }
}