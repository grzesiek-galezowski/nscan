using System;
using System.Linq;
using FluentAssertions;
using NScanSpecification.Lib.AutomationLayer;
using RunProcessAsTask;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class AnalysisResult
  {
    private ProcessResults? _analysisResult;

    private string ConsoleStandardOutput()
    {
      return string.Join(Environment.NewLine, _analysisResult.OrThrow().StandardOutput);
    }

    private string ConsoleStandardOutputAndErrorString()
    {
      var analysisResult = _analysisResult.OrThrow();
      return string.Join(Environment.NewLine, analysisResult.StandardOutput.Concat(analysisResult.StandardError));
    }

    public void ReportShouldNotContainText(string text)
    {
      ConsoleStandardOutput().Should().NotContain(text,
        ConsoleStandardOutputAndErrorString());
    }

    public void ShouldIndicateSuccess()
    {
      _analysisResult.OrThrow().ExitCode.Should().Be(0);
    }

    public void ShouldIndicateFailure()
    {
      _analysisResult!.ExitCode.Should().Be(-1);
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