using FluentAssertions;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.NScan.Domain;
using Xunit;
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
      //bug! rule description should be constructor parameter!!!
      report.AppendTo(resultBuilder);

      //THEN
      resultBuilder.Received(1).AppendOk(ruleDescription);
    }


    //bug more tests (AppendError etc.)
    //bug public void ShouldPrintAllPathViolationsInTheSameOrderTheyWereReceived()
    //bug public void ShouldAllowSeveralViolationsForTheSameRule()
    //bug public void ShouldPrintAViolationDescriptionOnlyOnceNoMatterHowManyTimesItWasReported()
  }
}
