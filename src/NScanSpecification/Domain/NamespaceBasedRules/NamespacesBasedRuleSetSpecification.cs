using System.Collections.Generic;
using NSubstitute;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.Domain.SharedKernel;
using Xunit;

namespace TddXt.NScan.Specification.Domain.NamespaceBasedRules
{
  public class NamespacesBasedRuleSetSpecification
  {
    [Fact]
    public void ShouldEvaluateEachProjectForEachRuleAndReportFinishAfterEachRuleCheckWhenChecked()
    {
      //GIVEN
      var ruleSet = new NamespacesBasedRuleSet();
      var report = Substitute.For<IAnalysisReportInProgress>();
      var project1 = Substitute.For<IDotNetProject>();
      var project2 = Substitute.For<IDotNetProject>();
      var project3 = Substitute.For<IDotNetProject>();
      var rule1 = AnyRoot.Root.Any.Instance<INamespacesBasedRule>();
      var rule2 = AnyRoot.Root.Any.Instance<INamespacesBasedRule>();
      var rule3 = AnyRoot.Root.Any.Instance<INamespacesBasedRule>();
      ruleSet.Add(rule1);
      ruleSet.Add(rule2);
      ruleSet.Add(rule3);

      //WHEN
      ruleSet.Check(new List<INamespaceBasedRuleTarget> {project1, project2, project3}, report);

      //THEN
      Received.InOrder(() =>
      {
        project1.Evaluate(rule1, report);
        project2.Evaluate(rule1, report);
        project3.Evaluate(rule1, report);
        report.FinishedChecking(rule1.Description());
        project1.Evaluate(rule2, report);
        project2.Evaluate(rule2, report);
        project3.Evaluate(rule2, report);
        report.FinishedChecking(rule2.Description());
        project1.Evaluate(rule3, report);
        project2.Evaluate(rule3, report);
        project3.Evaluate(rule3, report);
        report.FinishedChecking(rule3.Description());
      });
    }
  }
}