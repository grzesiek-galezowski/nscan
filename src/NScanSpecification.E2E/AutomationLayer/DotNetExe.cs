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

  private static async Task RunWith(string arguments, AbsoluteDirectoryPath workingDirectory,
    CancellationToken cancellationToken)
  {
    try
    {
      await Command.RunAsync("dotnet", arguments, workingDirectory.ToString(), cancellationToken: cancellationToken);
    }
    catch (ExitCodeException e)
    {
      throw new DotNetExeFailedException(arguments, workingDirectory, e.ExitCode, e);
    }
  }
}
