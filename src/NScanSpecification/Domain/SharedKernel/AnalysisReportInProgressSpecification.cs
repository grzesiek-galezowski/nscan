using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.SharedKernel;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.SharedKernel
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
      var report = new AnalysisReportInProgress();

      //WHEN
      var hasViolations = report.HasViolations();

      //THEN
      hasViolations.Should().BeFalse();
    }


      [Fact]
      public void ShouldPrintAllPathViolationsInTheSameOrderTheyWereReceived()
      {
        //GIVEN
        var report = new AnalysisReportInProgress();
        var violation1 = Any.Instance<RuleViolation>();
        var violation2 = Any.Instance<RuleViolation>();
        var violation3 = Any.Instance<RuleViolation>();

        report.Add(violation1);
        report.Add(violation2);
        report.Add(violation3);

        //WHEN
        var output = report.AsString();

        //THEN
        AssertContainsInOrder(output,
          $"{violation1.RuleDescription}: [ERROR]",
          violation1.ViolationDescription,
          $"{violation2.RuleDescription}: [ERROR]",
          violation2.ViolationDescription,
          $"{violation3.RuleDescription}: [ERROR]",
          violation3.ViolationDescription
        );
      }

      [Fact]
      public void ShouldAllowSeveralViolationsForTheSameRule()
      {
        //GIVEN
        var report = new AnalysisReportInProgress();

        var violation1 = Any.Instance<RuleViolation>();
        var violation2 = new RuleViolation(violation1.RuleDescription, Any.String(), Any.String());

        report.Add(violation1);
        report.Add(violation2);

        //WHEN
        var output = report.AsString();

        //THEN
        AssertContainsInOrder(output, ErrorHeaderWith(violation1.RuleDescription),
          violation1.ViolationDescription,
          violation2.ViolationDescription
        );
        AssertContainsOnce(output, ErrorHeaderWith(violation1.RuleDescription));
      }

      [Fact]
      public void ShouldPrintAViolationDescriptionOnlyOnceNoMatterHowManyTimesItWasReported()
      {
        //GIVEN
        var report = new AnalysisReportInProgress();

        var violation1 = Any.Instance<RuleViolation>();
        var violation2 = new RuleViolation(violation1.RuleDescription, violation1.PrefixPhrase, violation1.ViolationDescription);
        var violation3 = new RuleViolation(violation1.RuleDescription, violation1.PrefixPhrase, violation1.ViolationDescription);

        report.Add(violation1);
        report.Add(violation2);
        report.Add(violation3);

        //WHEN
        var output = report.AsString();

        //THEN
        AssertContainsInOrder(output, ErrorHeaderWith(violation1.RuleDescription),
          violation1.ViolationDescription
        );
        AssertContainsOnce(output, violation1.ViolationDescription);
      }

      [Fact]
      public void ShouldRespondItHasViolationsWhenAtLeastOneViolationWasAddedDespiteOtherOks()
      {
        //GIVEN
        var report = new AnalysisReportInProgress();

        report.Add(Any.Instance<RuleViolation>());
        report.FinishedChecking(Any.String());

        //WHEN
        var hasViolations = report.HasViolations();

        //THEN
        hasViolations.Should().BeTrue();
      }


      private static string ErrorHeaderWith(string anyDescription1)
      {
        return $"{anyDescription1}: [ERROR]";
      }

      //TODO move to X fluent assert
      private void AssertContainsInOrder(string output, params string[] subtexts)
      {
        var indices = subtexts.Select(subtext => output.IndexOf(subtext, StringComparison.Ordinal));

        indices.Should().NotContain(-1, output);
        indices.Should().BeInAscendingOrder(output);
      }

      //TODO move to X fluent assert
      private void AssertContainsOnce(string output, string substring)
      {
        IndexOfAll(output, substring).Should().HaveCount(1,
          "\"" + output + "\"" + " should contain exactly 1 occurence of " + "\"" + substring + "\"");
      }

      public static IEnumerable<int> IndexOfAll(string sourceString, string subString)
      {
        return Regex.Matches(sourceString, Regex.Escape(subString)).Select(m => m.Index);
      }
  }
}

