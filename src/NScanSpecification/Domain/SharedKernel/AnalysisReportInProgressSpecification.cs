using FluentAssertions;
using NScan.SharedKernel.SharedKernel;
using TddXt.AnyRoot.Strings;
using TddXt.XFluentAssert.Root;
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
        output.Should().ContainInOrder(
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
        output.Should().ContainInOrder(ErrorHeaderWith(violation1.RuleDescription),
          violation1.ViolationDescription,
          violation2.ViolationDescription
        );
        output.Should().ContainExactlyOnce(ErrorHeaderWith(violation1.RuleDescription));
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
        output.Should().ContainInOrder(ErrorHeaderWith(violation1.RuleDescription),
          violation1.ViolationDescription
        );
        output.Should().ContainExactlyOnce(violation1.ViolationDescription);
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
  }
}

