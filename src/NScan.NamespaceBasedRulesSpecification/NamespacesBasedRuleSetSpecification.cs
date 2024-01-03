using LanguageExt;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRulesSpecification;

public class NamespacesBasedRuleSetSpecification
{
  [Fact]
  public void ShouldEvaluateEachProjectForEachRuleAndReportFinishAfterEachRuleCheckWhenChecked()
  {
    //GIVEN
    var ruleSet = new NamespacesBasedRuleSet();
    var report = Substitute.For<IAnalysisReportInProgress>();
    var project1 = Substitute.For<INamespaceBasedRuleTarget>();
    var project2 = Substitute.For<INamespaceBasedRuleTarget>();
    var project3 = Substitute.For<INamespaceBasedRuleTarget>();
    var rule1 = Any.Instance<INamespacesBasedRule>();
    var rule2 = Any.Instance<INamespacesBasedRule>();
    var rule3 = Any.Instance<INamespacesBasedRule>();
    ruleSet.Add(rule1);
    ruleSet.Add(rule2);
    ruleSet.Add(rule3);
    var rule1Description = rule1.Description();
    var rule2Description = rule2.Description();
    var rule3Description = rule3.Description();

    //WHEN
    ruleSet.Check(Seq.create(project1, project2, project3), report);

    //THEN
    Received.InOrder(() =>
    {
      project1.Evaluate(rule1, report);
      project2.Evaluate(rule1, report);
      project3.Evaluate(rule1, report);
      report.FinishedEvaluatingRule(rule1Description);
      project1.Evaluate(rule2, report);
      project2.Evaluate(rule2, report);
      project3.Evaluate(rule2, report);
      report.FinishedEvaluatingRule(rule2Description);
      project1.Evaluate(rule3, report);
      project2.Evaluate(rule3, report);
      project3.Evaluate(rule3, report);
      report.FinishedEvaluatingRule(rule3Description);
    });
  }
}
