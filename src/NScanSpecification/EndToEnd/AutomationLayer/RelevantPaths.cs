using System.IO;
using System.Linq;
using System.Reflection;
using AtmaFileSystem;
using static AtmaFileSystem.AtmaFileSystemPaths;
using AbsoluteDirectoryPath = AtmaFileSystem.AbsoluteDirectoryPath;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public static class RelevantPaths
  {
    public static DirectoryInfo CreateRandomDirectory()
    {
      var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(tempDirectory);
      return new DirectoryInfo(tempDirectory);
    }

    public static AbsoluteDirectoryPath RepositoryPath()
    {
      if (NCrunch.RunsThisTest())
      {
        var originalSolutionPath = NCrunch.OriginalSolutionPath();
        return originalSolutionPath.FragmentEndingOnLast(DirectoryName("nscan")).Value;
      }
      else
      {
        var executingAssemblyPath = new FileInfo(
          Assembly.GetExecutingAssembly().EscapedCodeBase.Split("file:///").ToArray()[1]).Directory;
        while (!Directory.EnumerateDirectories(executingAssemblyPath.FullName).Any(s => s.EndsWith(".git")))
        {
          executingAssemblyPath = executingAssemblyPath.Parent;
        }

        return AbsoluteDirectoryPath(executingAssemblyPath.FullName);
      }

    }

    public static AbsoluteFilePath NscanConsoleProjectPath(AbsoluteDirectoryPath repositoryPath)
    {
      return 
        repositoryPath + DirectoryName("src") + DirectoryName("NScan.Console") + FileName("NScan.Console.csproj");
    }
  }
}