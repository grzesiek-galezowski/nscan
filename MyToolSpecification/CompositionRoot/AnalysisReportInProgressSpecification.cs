using System;
using System.Collections.Generic;
using FluentAssertions;
using MyTool.App;
using NSubstitute;
using TddXt.AnyRoot.Collections;
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
      var report = new AnalysisReportInProgress(Any.Instance<IProjectPathFormat>());
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

    [Fact]
    public void ShouldPrintAllViolationsInTheSameOrderTheyWereReceived()
    {
      //GIVEN
      var projectPathFormat = Substitute.For<IProjectPathFormat>();
      var report = new AnalysisReportInProgress(projectPathFormat);
      var anyDescription1 = Any.String();
      var anyDescription2 = Any.String();
      var anyDescription3 = Any.String();

      var violationPath1 = Any.List<IReferencedProject>();
      var violationPath2 = Any.List<IReferencedProject>();
      var violationPath3 = Any.List<IReferencedProject>();
      var formattedPath1 = Any.String();
      var formattedPath2 = Any.String();
      var formattedPath3 = Any.String();

      report.ViolationOf(anyDescription1, violationPath1);
      report.ViolationOf(anyDescription2, violationPath2);
      report.ViolationOf(anyDescription3, violationPath3);

      projectPathFormat.ApplyTo(violationPath1).Returns(formattedPath1);
      projectPathFormat.ApplyTo(violationPath2).Returns(formattedPath2);
      projectPathFormat.ApplyTo(violationPath3).Returns(formattedPath3);

      //WHEN
      var output = report.AsString();

      //THEN
      var indexOfDescription1 = output.IndexOf($"{anyDescription1}: [ERROR]");
      var indexOfPath1 = output.IndexOf(formattedPath1);
      var indexOfDescription2 = output.IndexOf($"{anyDescription2}: [ERROR]");
      var indexOfPath2 = output.IndexOf(formattedPath2);
      var indexOfDescription3 = output.IndexOf($"{anyDescription3}: [ERROR]");
      var indexOfPath3 = output.IndexOf(formattedPath3);
      indexOfDescription1.Should().BeGreaterThan(-1);
      indexOfPath1.Should().BeGreaterThan(indexOfDescription1);
      indexOfDescription2.Should().BeGreaterThan(indexOfPath1);
      indexOfPath2.Should().BeGreaterThan(indexOfDescription2);
      indexOfDescription3.Should().BeGreaterThan(indexOfPath2);
      indexOfPath3.Should().BeGreaterThan(indexOfDescription3);
    }

    //TODO what order of OKs and violations?
  }

}