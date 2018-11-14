using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
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

      projectPathFormat.ApplyTo(violationPath1).Returns(formattedPath1);
      projectPathFormat.ApplyTo(violationPath2).Returns(formattedPath2);
      projectPathFormat.ApplyTo(violationPath3).Returns(formattedPath3);

      report.Violation(anyDescription1, violationPath1);
      report.Violation(anyDescription2, violationPath2);
      report.Violation(anyDescription3, violationPath3);

      //WHEN
      var output = report.AsString();

      //THEN
      AssertContainsInOrder(output,
        $"{anyDescription1}: [ERROR]",
        formattedPath1,
        $"{anyDescription2}: [ERROR]",
        formattedPath2,
        $"{anyDescription3}: [ERROR]",
        formattedPath3
      );
    }

    [Fact]
    public void ShouldAllowSeveralViolationsForTheSameRule()
    {
      //GIVEN
      var projectPathFormat = Substitute.For<IProjectPathFormat>();
      var report = new AnalysisReportInProgress(projectPathFormat);
      var anyDescription1 = Any.String();

      var violationPath1 = Any.List<IReferencedProject>();
      var violationPath2 = Any.List<IReferencedProject>();
      var formattedPath1 = Any.String();
      var formattedPath2 = Any.String();

      projectPathFormat.ApplyTo(violationPath1).Returns(formattedPath1);
      projectPathFormat.ApplyTo(violationPath2).Returns(formattedPath2);

      report.Violation(anyDescription1, violationPath1);
      report.Violation(anyDescription1, violationPath2);

      //WHEN
      var output = report.AsString();

      //THEN
      AssertContainsInOrder(output,
        ErrorHeaderWith(anyDescription1),
        formattedPath1,
        formattedPath2
      );
      AssertContainsOnce(output, ErrorHeaderWith(anyDescription1));
    }

    [Fact]
    public void ShouldPrintAViolationPathOnlyOnceNoMatterHowManyTimesItWasReported()
    {
      //GIVEN
      var projectPathFormat = Substitute.For<IProjectPathFormat>();
      var report = new AnalysisReportInProgress(projectPathFormat);
      var anyDescription1 = Any.String();

      var violationPath1 = Any.List<IReferencedProject>();
      var violationPath2 = Any.List<IReferencedProject>();
      var violationPath3 = Any.List<IReferencedProject>();
      var formattedPath1 = Any.String();

      projectPathFormat.ApplyTo(violationPath1).Returns(formattedPath1);
      projectPathFormat.ApplyTo(violationPath2).Returns(formattedPath1);
      projectPathFormat.ApplyTo(violationPath3).Returns(formattedPath1);

      report.Violation(anyDescription1, violationPath1);
      report.Violation(anyDescription1, violationPath2);
      report.Violation(anyDescription1, violationPath3);

      //WHEN
      var output = report.AsString();

      //THEN
      AssertContainsInOrder(output,
        ErrorHeaderWith(anyDescription1),
        formattedPath1
      );
      AssertContainsOnce(output, formattedPath1);
    }

    [Fact]
    public void ShouldRespondItHasNoViolationsWhenNoViolationsWereAddedToIt()
    {
      //GIVEN
      var projectPathFormat = Substitute.For<IProjectPathFormat>();
      var report = new AnalysisReportInProgress(projectPathFormat);

      //WHEN
      var hasViolations = report.HasViolations();

      //THEN
      hasViolations.Should().BeFalse();
    }

    [Fact]
    public void ShouldRespondItHasNoViolationsWhenAtLeastOneViolationWasAddedDespiteOtherOks()
    {
      //GIVEN
      var projectPathFormat = Substitute.For<IProjectPathFormat>();
      var report = new AnalysisReportInProgress(projectPathFormat);

      report.Violation(Any.String(), Any.List<IReferencedProject>());
      report.Ok(Any.String());

      //WHEN
      var hasViolations = report.HasViolations();

      //THEN
      hasViolations.Should().BeTrue();
    }

    //bug combining OK and ERROR for the same rule should only give errors

    //TODO move to X fluent assert
    private void AssertContainsOnce(string output, string substring)
    {
      IndexOfAll(output, substring).Should().HaveCount(1, "\"" + output + "\"" + " should contain exactly 1 occurence of " + "\"" + substring + "\"");
    }

    private static string ErrorHeaderWith(string anyDescription1)
    {
      return $"{anyDescription1}: [ERROR]";
    }

    //TODO move to X fluent assert
    private void AssertContainsInOrder(string output, params string[] subtexts)
    {
      var indices = subtexts.Select(subtext => output.IndexOf((string) subtext, StringComparison.Ordinal)).ToList();

      indices.Should().NotContain(-1, output);
      indices.Should().BeInAscendingOrder(output);
    }

    public static IEnumerable<int> IndexOfAll(string sourceString, string subString)
    {
      return Regex.Matches(sourceString, Regex.Escape(subString)).Select(m => m.Index);
    }
  }

}