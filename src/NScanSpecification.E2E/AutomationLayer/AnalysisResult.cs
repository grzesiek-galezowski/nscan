using FluentAssertions;
using Core.NullableReferenceTypesExtensions;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class AnalysisResult
  {
    private string? _output;
    private int _returnCode = -100;

    private string ConsoleStandardOutput()
    {
      return _output.OrThrow();
    }

    public void ReportShouldNotContainText(string text)
    {
      ConsoleStandardOutput().Should().NotContain(text,
        ConsoleStandardOutput());
    }

    public void ShouldIndicateSuccess()
    {
      _returnCode.Should().Be(0);
    }

    public void ShouldIndicateFailure()
    {
      _returnCode.Should().Be(-1);
    }

    public void ReportShouldContainText(string ruleText)
    {
      ConsoleStandardOutput().Should()
        .Contain(ruleText,
          ConsoleStandardOutput());
    }

    public void Assign(int returnCode, string output)
    {
      _returnCode = returnCode;
      _output = output;
    }
  }
}
