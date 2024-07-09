using System.Text;
using AtmaFileSystem;
using AtmaFileSystem.IO;

namespace NScanSpecification.E2E.AutomationLayer;

public class DotNetExeFailedException(
  string arguments,
  AbsoluteDirectoryPath workingDirectory,
  int exitCode,
  Exception innerException)
  : Exception($"Running dotnet with arguments {arguments} " +
              $"in directory {workingDirectory} " +
              $"failed with code {exitCode}{Environment.NewLine}{ContentOf(workingDirectory)}", innerException)
{
  private static string ContentOf(AbsoluteDirectoryPath workingDirectory)
  {
    try
    {
      if (!workingDirectory.Exists())
      {
        return $"Directory {workingDirectory} does not exist";
      }
      else
      {
        var stringWithAllDirectoryEntries = new StringBuilder();
        foreach (var entry in workingDirectory.EnumerateFileSystemEntries())
        {
          stringWithAllDirectoryEntries.AppendLine(entry.ToString());
        }

        return stringWithAllDirectoryEntries.ToString();
      }
    }
    catch (Exception e)
    {
      return e.ToString();
    }
  }
}
