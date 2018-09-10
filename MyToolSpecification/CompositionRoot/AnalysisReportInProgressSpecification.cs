using System;
using FluentAssertions;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyTool.CompositionRoot
{
  public class AnalysisReportInProgressSpecification
  {
    [Fact]
    public void ShouldPrintAllOksInTheSameOrderTheyWereReceived()
    {
      //GIVEN
      var report = new AnalysisReportInProgress();
      var anyDescription1 = Any.String();
      var anyDescription2 = Any.String();
      var anyDescription3 = Any.String();

      report.Ok(anyDescription1);
      report.Ok(anyDescription2);
      report.Ok(anyDescription3);

      //WHEN
      var output = report.AsString();

      //THEN
      var indexOfDescription1 = output.IndexOf($"{anyDescription1}: [OK]");
      var indexOfDescription2 = output.IndexOf($"{anyDescription2}: [OK]");
      var indexOfDescription3 = output.IndexOf($"{anyDescription3}: [OK]");
      indexOfDescription1.Should().BeGreaterThan(-1);
      indexOfDescription2.Should().BeGreaterThan(indexOfDescription1);
      indexOfDescription3.Should().BeGreaterThan(indexOfDescription2);
    }

    //TODO what order of OKs and violations?
  }
}