using FluentAssertions;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using TddXt.XFluentAssert.Api;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Domain.SharedKernel
{
  public class AnalysisReportInProgressSpecification //bug replace builder with a mock
  {
    [Fact]
    public void ShouldPrintAllReportsInTheSameOrderTheyWereReceived()
    {
      //GIVEN
      var ruleReportFactory = Substitute.For<IRuleReportFactory>();
      var resultBuilder = Substitute.For<IResultBuilder>();
      var ruleDescription1 = Any.Instance<RuleDescription>();
      var ruleDescription2 = Any.Instance<RuleDescription>();
      var ruleDescription3 = Any.Instance<RuleDescription>();
      var report = new AnalysisReportInProgress(ruleReportFactory);
      var ruleReport1 = Substitute.For<IRuleReport>();
      var ruleReport2 = Substitute.For<IRuleReport>();
      var ruleReport3 = Substitute.For<IRuleReport>();

      ruleReportFactory.EmptyRuleReport().Returns(ruleReport1, ruleReport2, ruleReport3);

      report.FinishedEvaluatingRule(ruleDescription1);
      report.FinishedEvaluatingRule(ruleDescription2);
      report.FinishedEvaluatingRule(ruleDescription3);

      //WHEN
      report.AsString(resultBuilder);

      //THEN
      Received.InOrder(() =>
      {
        ruleReport1.AppendTo(resultBuilder, ruleDescription1);
        resultBuilder.AppendRuleSeparator();
        ruleReport2.AppendTo(resultBuilder, ruleDescription2);
        resultBuilder.AppendRuleSeparator();
        ruleReport3.AppendTo(resultBuilder, ruleDescription3);
      });
    }

    [Fact]
    public void ShouldNotReportSuccessWhenAtLeastOneSingleRuleDoesNotReportSuccess()
    {
      //GIVEN
      var ruleFactory = Substitute.For<IRuleReportFactory>();
      var report = new AnalysisReportInProgress(ruleFactory);
      var ruleReport1 = SuccessfulRuleReport();
      var ruleReport2 = FailedRuleReport();
      var ruleReport3 = SuccessfulRuleReport();

      ruleFactory.EmptyRuleReport().Returns(ruleReport1, ruleReport2, ruleReport3);

      report.FinishedEvaluatingRule(Any.Instance<RuleDescription>());
      report.FinishedEvaluatingRule(Any.Instance<RuleDescription>());
      report.FinishedEvaluatingRule(Any.Instance<RuleDescription>());

      //WHEN
      var isSuccessful = report.IsSuccessful();

      //THEN
      isSuccessful.Should().BeFalse();
    }
    
    [Fact]
    public void ShouldAddViolationsForEachNewRuleToReportCreatedForThatRule()
    {
      //GIVEN
      var ruleFactory = Substitute.For<IRuleReportFactory>();
      var report = new AnalysisReportInProgress(ruleFactory);
      var ruleReport1 = Substitute.For<IRuleReport>();
      var ruleReport2 = Substitute.For<IRuleReport>();
      var ruleReport3 = Substitute.For<IRuleReport>();
      var rule1Description = Any.Instance<RuleDescription>();
      var rule2Description = Any.Instance<RuleDescription>();
      var rule3Description = Any.Instance<RuleDescription>();
      var rule1Violation1 = Any.Instance<RuleViolation>() with {RuleDescription = rule1Description};
      var rule1Violation2 = Any.Instance<RuleViolation>() with {RuleDescription = rule1Description};
      var rule2Violation1 = Any.Instance<RuleViolation>() with {RuleDescription = rule2Description};
      var rule2Violation2 = Any.Instance<RuleViolation>() with {RuleDescription = rule2Description};
      var rule3Violation1 = Any.Instance<RuleViolation>() with {RuleDescription = rule3Description};
      var rule3Violation2 = Any.Instance<RuleViolation>() with {RuleDescription = rule3Description};

      ruleFactory.EmptyRuleReport().Returns(ruleReport1, ruleReport2, ruleReport3);

      //WHEN
      report.Add(rule1Violation1);
      report.Add(rule1Violation2);
      report.Add(rule2Violation1);
      report.Add(rule2Violation2);
      report.Add(rule3Violation1);
      report.Add(rule3Violation2);

      //THEN
      ruleReport1.Received(1).AddViolation(rule1Violation1);
      ruleReport1.Received(1).AddViolation(rule1Violation2);
      ruleReport2.Received(1).AddViolation(rule2Violation1);
      ruleReport2.Received(1).AddViolation(rule2Violation2);
      ruleReport3.Received(1).AddViolation(rule3Violation1);
      ruleReport3.Received(1).AddViolation(rule3Violation2);
    }

    [Fact]
    public void ShouldReportSuccessWhenAllSingleRuleReportsReportSuccess()
    {
      //GIVEN
      var ruleFactory = Substitute.For<IRuleReportFactory>();
      var report = new AnalysisReportInProgress(ruleFactory);
      var report1 = SuccessfulRuleReport();
      var report2 = SuccessfulRuleReport();
      var report3 = SuccessfulRuleReport();

      ruleFactory.EmptyRuleReport().Returns(report1, report2, report3);

      //WHEN
      var isSuccessful = report.IsSuccessful();

      //THEN
      isSuccessful.Should().BeTrue();
    }

    //bug public void ShouldPrintAllPathViolationsInTheSameOrderTheyWereReceived()
    //bug public void ShouldAllowSeveralViolationsForTheSameRule()
    //bug public void ShouldPrintAViolationDescriptionOnlyOnceNoMatterHowManyTimesItWasReported()

    private static IRuleReport SuccessfulRuleReport()
    {
      var report = Substitute.For<IRuleReport>();
      report.IsSuccessful().Returns(true);
      return report;
    }

    private static IRuleReport FailedRuleReport()
    {
      var report = Substitute.For<IRuleReport>();
      report.IsSuccessful().Returns(false);
      return report;
    }
  }
}
