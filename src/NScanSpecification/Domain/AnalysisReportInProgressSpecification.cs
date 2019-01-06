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

      report.FinishedChecking(anyDescription1);
      report.FinishedChecking(anyDescription2);
      report.FinishedChecking(anyDescription3);

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


    public class PathViolationsSpecification
    {
      [Fact]
      public void ShouldPrintAllPathViolationsInTheSameOrderTheyWereReceived()
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

        report.PathViolation(anyDescription1, violationPath1);
        report.PathViolation(anyDescription2, violationPath2);
        report.PathViolation(anyDescription3, violationPath3);

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

        report.PathViolation(anyDescription1, violationPath1);
        report.PathViolation(anyDescription1, violationPath2);

        //WHEN
        var output = report.AsString();

        //THEN
        AssertContainsInOrder(output, ErrorHeaderWith(anyDescription1),
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

        report.PathViolation(anyDescription1, violationPath1);
        report.PathViolation(anyDescription1, violationPath2);
        report.PathViolation(anyDescription1, violationPath3);

        //WHEN
        var output = report.AsString();

        //THEN
        AssertContainsInOrder(output, ErrorHeaderWith(anyDescription1),
          formattedPath1
        );
        AssertContainsOnce(output, formattedPath1);
      }

      [Fact]
      public void ShouldRespondItHasViolationsWhenAtLeastOneViolationWasAddedDespiteOtherOks()
      {
        //GIVEN
        var projectPathFormat = Substitute.For<IProjectPathFormat>();
        var report = new AnalysisReportInProgress(projectPathFormat);

        report.PathViolation(Any.String(), Any.List<IReferencedProject>());
        report.FinishedChecking(Any.String());

        //WHEN
        var hasViolations = report.HasViolations();

        //THEN
        hasViolations.Should().BeTrue();
      }

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
        var indices = subtexts.Select(subtext => output.IndexOf((string) subtext, StringComparison.Ordinal));

        indices.Should().NotContain(-1, output);
        indices.Should().BeInAscendingOrder(output);
      }

      public static IEnumerable<int> IndexOfAll(string sourceString, string subString)
      {
        return Regex.Matches(sourceString, Regex.Escape(subString)).Select(m => m.Index);
      }
    }

    public class ProjectScopedViolationsSpecification
    {

      [Fact]
      public void ShouldPrintAllViolationsInTheSameOrderTheyWereReceived()
      {
        //GIVEN
        var projectPathFormat = Substitute.For<IProjectPathFormat>();
        var report = new AnalysisReportInProgress(projectPathFormat);
        var ruleDescription1 = Any.String();
        var ruleDescription2 = Any.String();
        var ruleDescription3 = Any.String();
        var violationDescription1 = Any.String();
        var violationDescription2 = Any.String();
        var violationDescription3 = Any.String();

        report.ProjectScopedViolation(ruleDescription1, violationDescription1);
        report.ProjectScopedViolation(ruleDescription2, violationDescription2);
        report.ProjectScopedViolation(ruleDescription3, violationDescription3);

        //WHEN
        var output = report.AsString();

        //THEN
        AssertContainsInOrder(output,
          $"{ruleDescription1}: [ERROR]",
          violationDescription1,
          $"{ruleDescription2}: [ERROR]",
          violationDescription2,
          $"{ruleDescription3}: [ERROR]",
          violationDescription3
        );
      }

      [Fact]
      public void ShouldAllowSeveralViolationsForTheSameRule()
      {
        //GIVEN
        var projectPathFormat = Substitute.For<IProjectPathFormat>();
        var report = new AnalysisReportInProgress(projectPathFormat);
        var anyDescription1 = Any.String();

        var violationDescription1 = Any.String();
        var violationDescription2 = Any.String();

        report.ProjectScopedViolation(anyDescription1, violationDescription1);
        report.ProjectScopedViolation(anyDescription1, violationDescription2);

        //WHEN
        var output = report.AsString();

        //THEN
        AssertContainsInOrder(output, ErrorHeaderWith(anyDescription1),
          violationDescription1,
          violationDescription2
        );
        AssertContainsOnce(output, ErrorHeaderWith(anyDescription1));
      }

      [Fact]
      public void ShouldPrintAViolationDescriptionOnlyOnceNoMatterHowManyTimesItWasReported()
      {
        //GIVEN
        var projectPathFormat = Substitute.For<IProjectPathFormat>();
        var report = new AnalysisReportInProgress(projectPathFormat);
        var ruleDescription = Any.String();
        var violationDescription = Any.String();

        report.ProjectScopedViolation(ruleDescription, violationDescription);
        report.ProjectScopedViolation(ruleDescription, violationDescription);
        report.ProjectScopedViolation(ruleDescription, violationDescription);

        //WHEN
        var output = report.AsString();

        //THEN
        AssertContainsInOrder(output, 
          ErrorHeaderWith(ruleDescription),
          violationDescription
        );
        AssertContainsOnce(output, violationDescription);
      }

      [Fact]
      public void ShouldRespondItHasNoViolationsWhenAtLeastOneViolationWasAddedDespiteOtherOks()
      {
        //GIVEN
        var projectPathFormat = Substitute.For<IProjectPathFormat>();
        var report = new AnalysisReportInProgress(projectPathFormat);

        report.ProjectScopedViolation(Any.String(), Any.String());
        report.FinishedChecking(Any.String());

        //WHEN
        var hasViolations = report.HasViolations();

        //THEN
        hasViolations.Should().BeTrue();
      }

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
        var indices = subtexts.Select(subtext => output.IndexOf((string) subtext, StringComparison.Ordinal));

        indices.Should().NotContain(-1, output);
        indices.Should().BeInAscendingOrder(output);
      }

      public static IEnumerable<int> IndexOfAll(string sourceString, string subString)
      {
        return Regex.Matches(sourceString, Regex.Escape(subString)).Select(m => m.Index);
      }
    }
  }

}