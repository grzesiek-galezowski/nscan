using System.Threading;
using AtmaFileSystem;
using SimpleExec;

namespace NScanSpecification.E2E.AutomationLayer;

public class DotNetExe(SolutionDir workingDirectory, ITestSupport testSupport)
{
  public async Task RunWith(string arguments, CancellationToken cancellationToken)
  {
    workingDirectory.AssertExists();
    testSupport.RunningDotnetExeWith(arguments, workingDirectory);
    await RunWith(arguments, workingDirectory.FullName(), cancellationToken);
  }

  private async Task RunWith(string arguments, AbsoluteDirectoryPath workingDirectory,
    CancellationToken cancellationToken)
  {
    try
    {
      var exitCode = 0;
      string standardError = "";
      string standardOutput = "";
      do
      {
        (standardOutput, standardError) = await Command.ReadAsync("dotnet", arguments, workingDirectory.ToString(), cancellationToken: cancellationToken, handleExitCode: i =>
        {
          exitCode = i;
          return true;
        });
        testSupport.DotnetExeFinished(exitCode, standardOutput, standardError);
      } while (standardError.Contains("because it is being used by another process."));
      if (exitCode != 0)
      {
        throw new ExitCodeException(exitCode);
      }
    }
    catch (ExitCodeException e)
    {
      throw new DotNetExeFailedException(arguments, workingDirectory, e.ExitCode, e);
    }
  }
}
