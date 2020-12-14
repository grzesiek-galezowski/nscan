using System.Diagnostics;
using System.Threading.Tasks;
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
      var processInfo = await ProcessEx.RunAsync(
        new ProcessStartInfo("dotnet.exe", arguments)
        {
          WorkingDirectory = _workingDirectory.FullName().ToString(),
        }).ConfigureAwait(false);

      return processInfo;
    }
  }
}