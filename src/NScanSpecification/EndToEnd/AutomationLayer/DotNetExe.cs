using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using RunProcessAsTask;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class DotNetExe
  {
    private readonly DirectoryInfo _workingDirectory;

    public DotNetExe(DirectoryInfo workingDirectory)
    {
      _workingDirectory = workingDirectory;
    }

    public async Task<ProcessResults> RunWith(string arguments)
    {
      var processInfo = await ProcessEx.RunAsync(
        new ProcessStartInfo("dotnet.exe", arguments)
        {
            
          WorkingDirectory = _workingDirectory.FullName,
        }).ConfigureAwait(false);
      return processInfo;
    }
  }
}