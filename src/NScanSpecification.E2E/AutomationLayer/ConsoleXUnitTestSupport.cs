using Xunit.Abstractions;

namespace NScanSpecification.E2E.AutomationLayer
{
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
  }
}