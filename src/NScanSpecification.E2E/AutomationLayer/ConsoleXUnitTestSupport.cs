using AtmaFileSystem;

namespace NScanSpecification.E2E.AutomationLayer;

internal class ConsoleXUnitTestSupport(ITestOutputHelper output) : ITestSupport
{
  public void RunningDotnetExeWith(string arguments, SolutionDir workingDirectory)
  {
    output.WriteLine($"Running dotnet.exe {arguments} in {workingDirectory.FullName()}");
  }

  public void DotnetExeFinished(int exitCode, string standardOutput, string standardError)
  {
    output.WriteLine($"dotnet.exe finished with exit code {exitCode}.{Environment.NewLine}StandardOutput: {standardOutput},{Environment.NewLine}StandardError {standardError}");
  }

  public void DeletingFile(AbsoluteFilePath redundantGeneratedFile)
  {
    output.WriteLine($"Deleting {redundantGeneratedFile}");
  }

  public void DeletedFile(AbsoluteFilePath redundantGeneratedFile)
  {
    output.WriteLine($"Deleted {redundantGeneratedFile}");
  }

  public void CreatingProject(AbsoluteFilePath projectPath)
  {
    output.WriteLine($"Creating {projectPath}");
  }

  public void CreatedProject(AbsoluteFilePath projectPath)
  {
    output.WriteLine($"Created {projectPath}");
  }
}
