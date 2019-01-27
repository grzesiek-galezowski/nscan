using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Primitives;
using RunProcessAsTask;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class AnalysisResult
  {
    private ProcessResults _analysisResult;

    private string ConsoleStandardOutput()
    {
      return string.Join(Environment.NewLine, _analysisResult.StandardOutput);
    }

    private string ConsoleStandardOutputAndErrorString()
    {
      return string.Join(Environment.NewLine, _analysisResult.StandardOutput.Concat(_analysisResult.StandardError));
    }

    public void ReportShouldNotContainText(string text)
    {
      ConsoleStandardOutput().Should().NotContain(text,
        ConsoleStandardOutputAndErrorString());
    }

    public void ShouldIndicateSuccess()
    {
      _analysisResult.ExitCode.Should().Be(0);
    }

    public void ShouldIndicateFailure()
    {
      _analysisResult.ExitCode.Should().Be(-1);
    }

    public void ReportShouldContainText(string ruleText)
    {
      ConsoleStandardOutput().Should()
        .Contain(ruleText,
          ConsoleStandardOutputAndErrorString());
    }

    public void Assign(ProcessResults analysisResultAnalysisResult)
    {
      _analysisResult = analysisResultAnalysisResult;
    }
  }
}