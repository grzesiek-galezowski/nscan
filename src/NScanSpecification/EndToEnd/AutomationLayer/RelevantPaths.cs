using System.IO;
using System.Linq;
using System.Reflection;
using AtmaFileSystem;

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
        return originalSolutionPath.SplitToIncluding("nscan"); //bug
      }
      else
      {
        var executingAssemblyPath = new FileInfo(
          Assembly.GetExecutingAssembly().EscapedCodeBase.Split("file:///").ToArray()[1]).Directory;
        while (!Directory.EnumerateDirectories(executingAssemblyPath.FullName).Any(s => s.EndsWith(".git")))
        {
          executingAssemblyPath = executingAssemblyPath.Parent;
        }

        return AbsoluteDirectoryPath.Value(executingAssemblyPath.FullName);
      }

    }

    private static AbsoluteDirectoryPath SplitToIncluding(this AbsoluteDirectoryPath originalSolutionPath, string dirName)
    {
      return AbsoluteDirectoryPath.Value(Path.Combine(originalSolutionPath.ToString().Split(dirName).First(), dirName));
    }

    public static AbsoluteFilePath NscanConsoleProjectPath(AbsoluteDirectoryPath repositoryPath)
    {
      return 
        repositoryPath + DirectoryName.Value("src") + DirectoryName.Value("NScan.Console") + FileName.Value("NScan.Console.csproj");
    }
  }
}