using System;

namespace NScanSpecification.E2E.AutomationLayer
{
  internal class ConsoleTestSupport : ITestSupport
  {
    public void RunningDotnetExeWith(string arguments, SolutionDir workingDirectory)
    {
      Console.WriteLine($"Running dotnet.exe {arguments} in {workingDirectory.FullName()}");
    }
  }
}