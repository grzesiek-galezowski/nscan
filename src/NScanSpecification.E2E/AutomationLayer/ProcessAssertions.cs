using System;
using System.Linq;
using FluentAssertions;
using RunProcessAsTask;

namespace NScanSpecification.E2E.AutomationLayer;

static internal class ProcessAssertions
{
  public static void AssertSuccess(ProcessResults processInfo)
  {
    processInfo.ExitCode.Should().Be(0, String.Join(Environment.NewLine, processInfo.StandardError.Concat(processInfo.StandardOutput)));
  }
}