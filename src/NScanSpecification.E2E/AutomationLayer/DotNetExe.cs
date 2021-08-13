using System.Diagnostics;
using System.Threading.Tasks;
using AtmaFileSystem;
using RunProcessAsTask;
using SimpleExec;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class DotNetExe
  {
    private readonly SolutionDir _workingDirectory;
    private readonly ITestSupport _testSupport;

    public DotNetExe(SolutionDir workingDirectory, ITestSupport testSupport)
    {
      _workingDirectory = workingDirectory;
      _testSupport = testSupport;
    }

    public async Task RunWith(string arguments)
    {
      _workingDirectory.AssertExists();
      _testSupport.RunningDotnetExeWith(arguments, _workingDirectory);
      await RunWith(arguments, _workingDirectory.FullName());
    }

    public static async Task RunWith(string arguments, AbsoluteDirectoryPath workingDirectory)
    {
      await Command.RunAsync("dotnet.exe", arguments, workingDirectory: workingDirectory.ToString());
    }
  }
}
