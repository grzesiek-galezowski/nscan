using FluentAssertions;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.NScan.Domain;
using Xunit;
using static System.Environment;
using static TddXt.AnyRoot.Root;

namespace NScan.MainSpecification.Domain
{
  public class SingleRuleReportSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenNoViolationsWereAdded()
    {
      //GIVEN
      var report = new SingleRuleReport(Any.Instance<RuleDescription>());

      //WHEN
      var isFailed = report.IsFailed();

      //THEN
      isFailed.Should().BeFalse();
    }

    [Fact]
    public void ShouldReportFailureWhenAtLeastOneViolationWasAdded()
    {
      //GIVEN
      var report = new SingleRuleReport(Any.Instance<RuleDescription>());

      report.Add(Any.Instance<RuleViolation>());

      //WHEN
      var isFailed = report.IsFailed();

      //THEN
      isFailed.Should().BeTrue();
    }

    [Fact]
    public void ShouldAppendSuccessToResultBuilderWhenNoViolationsWereReported()
    {
      //GIVEN
      var ruleDescription = Any.Instance<RuleDescription>();
      var report = new SingleRuleReport(ruleDescription);
      var resultBuilder = Substitute.For<IResultBuilder>();

      //WHEN
      report.AppendTo(resultBuilder);

      //THEN
      resultBuilder.Received(1).AppendOk(ruleDescription);
    }
    
    [Fact]
    public void ShouldAppendFailureToResultBuilderWhenAtLeastOneViolationIsReported()
    {
      //GIVEN
      var ruleDescription = Any.Instance<RuleDescription>();
      var report = new SingleRuleReport(ruleDescription);
      var resultBuilder = Substitute.For<IResultBuilder>();
      var ruleViolation = Any.Instance<RuleViolation>();

      report.Add(ruleViolation);

      //WHEN
      report.AppendTo(resultBuilder);

      //THEN
      resultBuilder.Received(1).AppendViolations(ruleDescription, ruleViolation.ToHumanReadableString());
    }

    [Fact]
    public void ShouldAppendFailureWithAllReportedViolationsInTheSameOrderTheyWereReceivedToResultBuilder()
    {
      //GIVEN
      var ruleDescription = Any.Instance<RuleDescription>();
      var report = new SingleRuleReport(ruleDescription);
      var resultBuilder = Substitute.For<IResultBuilder>();
      var ruleViolation1 = Any.Instance<RuleViolation>();
      var ruleViolation2 = Any.Instance<RuleViolation>();
      var ruleViolation3 = Any.Instance<RuleViolation>();

      report.Add(ruleViolation1);
      report.Add(ruleViolation2);
      report.Add(ruleViolation3);

      //WHEN
      report.AppendTo(resultBuilder);

      //THEN
      resultBuilder.Received(1).AppendViolations(ruleDescription, 
        $"{ruleViolation1.ToHumanReadableString()}{NewLine}" +
        $"{ruleViolation2.ToHumanReadableString()}{NewLine}" +
        $"{ruleViolation3.ToHumanReadableString()}");
    }

    [Fact]
    public void ShouldAppendTheSameViolationDescriptionOnlyOnceNoMatterHowManyTimesItWasReported()
    {
      //GIVEN
      var ruleDescription = Any.Instance<RuleDescription>();
      var report = new SingleRuleReport(ruleDescription);
      var resultBuilder = Substitute.For<IResultBuilder>();
      var ruleViolation1 = Any.Instance<RuleViolation>();

      report.Add(ruleViolation1);
      report.Add(ruleViolation1);
      report.Add(ruleViolation1);

      //WHEN
      report.AppendTo(resultBuilder);

      //THEN
      resultBuilder.Received(1).AppendViolations(
        ruleDescription, 
        ruleViolation1.ToHumanReadableString());
    }


    //bug more tests (AppendError etc.)
    //bug public void ShouldPrintAViolationDescriptionOnlyOnceNoMatterHowManyTimesItWasReported()
  }
}
