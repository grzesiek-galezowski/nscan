using System.Diagnostics;
using System.Threading.Tasks;
using RunProcessAsTask;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class DotNetExe
  {
    private readonly SolutionDir _workingDirectory;

    public DotNetExe(SolutionDir workingDirectory)
    {
      _workingDirectory = workingDirectory;
    }

    public async Task<ProcessResults> RunWith(string arguments)
    {
      var processInfo = await ProcessEx.RunAsync(
        new ProcessStartInfo("dotnet.exe", arguments)
        {
          WorkingDirectory = _workingDirectory.FullName().ToString(),
        }).ConfigureAwait(false);
      return processInfo;
    }
  }
}