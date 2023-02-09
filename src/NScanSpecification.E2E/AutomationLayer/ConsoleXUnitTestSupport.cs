using System;

namespace NScanSpecification.E2E.AutomationLayer;

internal class ConsoleXUnitTestSupport : ITestSupport
{
  private readonly ITestOutputHelper _output;

  public ConsoleXUnitTestSupport(ITestOutputHelper output)
  {
    _output = output;
  }

  public void RunningDotnetExeWith(string arguments, SolutionDir workingDirectory)
  {
    _output.WriteLine($"Running dotnet.exe {arguments} in {workingDirectory.FullName()}");
  }

  public void DotnetExeFinished(int exitCode, string standardOutput, string standardError)
  {
    _output.WriteLine($"dotnet.exe finished with exit code {exitCode}.{Environment.NewLine}StandardOutput: {standardOutput},{Environment.NewLine}StandardError {standardError}");
  }
}
