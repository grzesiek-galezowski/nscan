using System.Threading;
using System.Threading.Tasks;
using AtmaFileSystem;
using SimpleExec;

namespace NScanSpecification.E2E.AutomationLayer;

public class DotNetExe
{
  private readonly SolutionDir _workingDirectory;
  private readonly ITestSupport _testSupport;

  public DotNetExe(SolutionDir workingDirectory, ITestSupport testSupport)
  {
    _workingDirectory = workingDirectory;
    _testSupport = testSupport;
  }

  public async Task RunWith(string arguments, CancellationToken cancellationToken)
  {
    _workingDirectory.AssertExists();
    _testSupport.RunningDotnetExeWith(arguments, _workingDirectory);
    await RunWith(arguments, _workingDirectory.FullName(), cancellationToken);
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
        _testSupport.DotnetExeFinished(exitCode, standardOutput, standardError);
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
