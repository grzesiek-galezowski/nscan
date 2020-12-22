using System.Diagnostics;
using System.Threading.Tasks;
using AtmaFileSystem;
using RunProcessAsTask;

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

    public async Task<ProcessResults> RunWith(string arguments)
    {
      _testSupport.RunningDotnetExeWith(arguments, _workingDirectory);
      return await RunWith(arguments, _workingDirectory.FullName());
    }

    public static async Task<ProcessResults> RunWith(string arguments, AbsoluteDirectoryPath workingDirectory)
    {
      var processInfo = await ProcessEx.RunAsync(
        new ProcessStartInfo("dotnet.exe", arguments)
        {
          WorkingDirectory = workingDirectory.ToString(),
        }).ConfigureAwait(false);

      return processInfo;
    }
  }
}
