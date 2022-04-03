using NScan.SharedKernel;
using TddXt.NScan.Domain;

namespace NScan.MainSpecification.Domain;

public class AnalysisReportInProgressSpecification
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
    var ruleReport1 = Substitute.For<ISingleRuleReport>();
    var ruleReport2 = Substitute.For<ISingleRuleReport>();
    var ruleReport3 = Substitute.For<ISingleRuleReport>();

    ruleReportFactory.EmptyReportFor(ruleDescription1).Returns(ruleReport1);
    ruleReportFactory.EmptyReportFor(ruleDescription2).Returns(ruleReport2);
    ruleReportFactory.EmptyReportFor(ruleDescription3).Returns(ruleReport3);

    report.FinishedEvaluatingRule(ruleDescription1);
    report.FinishedEvaluatingRule(ruleDescription2);
    report.FinishedEvaluatingRule(ruleDescription3);

    //WHEN
    report.PutContentInto(resultBuilder);

    //THEN
    Received.InOrder(() =>
    {
      ruleReport1.AppendTo(resultBuilder);
      resultBuilder.AppendRuleSeparator();
      ruleReport2.AppendTo(resultBuilder);
      resultBuilder.AppendRuleSeparator();
      ruleReport3.AppendTo(resultBuilder);
    });
  }

  [Fact]
  public void ShouldReportFailureWhenAtLeastOneSingleRuleReportsFailure()
  {
    //GIVEN
    var ruleFactory = Substitute.For<IRuleReportFactory>();
    var report = new AnalysisReportInProgress(ruleFactory);
    var ruleDescription1 = Any.Instance<RuleDescription>();
    var ruleDescription2 = Any.Instance<RuleDescription>();
    var ruleDescription3 = Any.Instance<RuleDescription>();
    var ruleReport1 = SuccessfulRuleReport();
    var ruleReport2 = FailedRuleReport();
    var ruleReport3 = SuccessfulRuleReport();

    ruleFactory.EmptyReportFor(ruleDescription1).Returns(ruleReport1);
    ruleFactory.EmptyReportFor(ruleDescription2).Returns(ruleReport2);
    ruleFactory.EmptyReportFor(ruleDescription3).Returns(ruleReport3);

    report.FinishedEvaluatingRule(ruleDescription1);
    report.FinishedEvaluatingRule(ruleDescription2);
    report.FinishedEvaluatingRule(ruleDescription3);

    //WHEN
    var isFailure = report.IsFailure();

    //THEN
    isFailure.Should().BeTrue();
  }
    
  [Fact]
  public void ShouldReportSuccessWhenAllSingleRuleReportsReportSuccess()
  {
    //GIVEN
    var ruleFactory = Substitute.For<IRuleReportFactory>();
    var report = new AnalysisReportInProgress(ruleFactory);
    var ruleDescription1 = Any.Instance<RuleDescription>();
    var ruleDescription2 = Any.Instance<RuleDescription>();
    var ruleDescription3 = Any.Instance<RuleDescription>();
    var singleRuleReport1 = SuccessfulRuleReport();
    var singleRuleReport2 = SuccessfulRuleReport();
    var singleRuleReport3 = SuccessfulRuleReport();

    ruleFactory.EmptyReportFor(ruleDescription1).Returns(singleRuleReport1);
    ruleFactory.EmptyReportFor(ruleDescription2).Returns(singleRuleReport2);
    ruleFactory.EmptyReportFor(ruleDescription3).Returns(singleRuleReport3);

    report.FinishedEvaluatingRule(ruleDescription1);
    report.FinishedEvaluatingRule(ruleDescription2);
    report.FinishedEvaluatingRule(ruleDescription3);

    //WHEN
    var isFailure = report.IsFailure();

    //THEN
    isFailure.Should().BeFalse();
  }

  [Fact]
  public void ShouldAddViolationsForEachNewRuleToReportCreatedForThatRule()
  {
    //GIVEN
    var ruleFactory = Substitute.For<IRuleReportFactory>();
    var report = new AnalysisReportInProgress(ruleFactory);
    var ruleReport1 = Substitute.For<ISingleRuleReport>();
    var ruleReport2 = Substitute.For<ISingleRuleReport>();
    var ruleReport3 = Substitute.For<ISingleRuleReport>();
    var rule1Description = Any.Instance<RuleDescription>();
    var rule2Description = Any.Instance<RuleDescription>();
    var rule3Description = Any.Instance<RuleDescription>();
    var rule1Violation1 = Any.Instance<RuleViolation>() with {RuleDescription = rule1Description};
    var rule1Violation2 = Any.Instance<RuleViolation>() with {RuleDescription = rule1Description};
    var rule2Violation1 = Any.Instance<RuleViolation>() with {RuleDescription = rule2Description};
    var rule2Violation2 = Any.Instance<RuleViolation>() with {RuleDescription = rule2Description};
    var rule3Violation1 = Any.Instance<RuleViolation>() with {RuleDescription = rule3Description};
    var rule3Violation2 = Any.Instance<RuleViolation>() with {RuleDescription = rule3Description};

    ruleFactory.EmptyReportFor(rule1Description).Returns(ruleReport1);
    ruleFactory.EmptyReportFor(rule2Description).Returns(ruleReport2);
    ruleFactory.EmptyReportFor(rule3Description).Returns(ruleReport3);

    //WHEN
    report.Add(rule1Violation1);
    report.Add(rule1Violation2);
    report.Add(rule2Violation1);
    report.Add(rule2Violation2);
    report.Add(rule3Violation1);
    report.Add(rule3Violation2);

    //THEN
    ruleReport1.Received(1).Add(rule1Violation1);
    ruleReport1.Received(1).Add(rule1Violation2);
    ruleReport2.Received(1).Add(rule2Violation1);
    ruleReport2.Received(1).Add(rule2Violation2);
    ruleReport3.Received(1).Add(rule3Violation1);
    ruleReport3.Received(1).Add(rule3Violation2);
  }

  private static ISingleRuleReport FailedRuleReport()
  {
    var report = Substitute.For<ISingleRuleReport>();
    report.IsFailed().Returns(true);
    return report;
  }

  private static ISingleRuleReport SuccessfulRuleReport()
  {
    var report = Substitute.For<ISingleRuleReport>();
    report.IsFailed().Returns(false);
    return report;
  }
}
