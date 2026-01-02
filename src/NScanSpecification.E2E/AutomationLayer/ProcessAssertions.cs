using System.Linq;
using AwesomeAssertions;
using RunProcessAsTask;

namespace NScanSpecification.E2E.AutomationLayer;

internal static class ProcessAssertions
{
  public static void AssertSuccess(ProcessResults processInfo)
  {
    processInfo.ExitCode.Should().Be(0, String.Join(Environment.NewLine, processInfo.StandardError.Concat(processInfo.StandardOutput)));
  }
}
