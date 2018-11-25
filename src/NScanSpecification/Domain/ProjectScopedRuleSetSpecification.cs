using System;
using System.Collections.Generic;
using System.Data;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.NScan.Domain;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class ProjectScopedRuleSetSpecification
  {
    [Fact]
    public void ShouldMakeEachProjectCheckEachRule()
    {
      //GIVEN
      var ruleSet = new ProjectScopedRuleSet();
      var rule1 = Any.Instance<IProjectScopedRule>();
      var rule2 = Any.Instance<IProjectScopedRule>();
      var rule3 = Any.Instance<IProjectScopedRule>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      var project1 = Substitute.For<IProjectScopedRuleTarget>();
      var project2 = Substitute.For<IProjectScopedRuleTarget>();
      var project3 = Substitute.For<IProjectScopedRuleTarget>();

      ruleSet.Add(rule1);
      ruleSet.Add(rule2);
      ruleSet.Add(rule3);

      //WHEN
      ruleSet.Check(new [] { project1, project2, project3 }, report);
      
      //THEN
      project1.Received(1).Evaluate(rule1, report);
      project1.Received(1).Evaluate(rule2, report);
      project1.Received(1).Evaluate(rule3, report);
      project2.Received(1).Evaluate(rule1, report);
      project2.Received(1).Evaluate(rule2, report);
      project2.Received(1).Evaluate(rule3, report);
      project3.Received(1).Evaluate(rule1, report);
      project3.Received(1).Evaluate(rule2, report);
      project3.Received(1).Evaluate(rule3, report);
    }
  }
}