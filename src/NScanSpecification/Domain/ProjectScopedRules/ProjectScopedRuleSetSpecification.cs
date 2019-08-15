using NScan.Domain.Domain.ProjectScopedRules;
using NScan.SharedKernel.SharedKernel;
using NSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.ProjectScopedRules
{
  public class ProjectScopedRuleSetSpecification
  {
    [Fact]
    public void ShouldMakeEachProjectCheckEachRule()
    {
      //GIVEN
      var ruleSet = new ProjectScopedRuleSet();
      var rule1 = Substitute.For<IProjectScopedRule>();
      var rule2 = Substitute.For<IProjectScopedRule>();
      var rule3 = Substitute.For<IProjectScopedRule>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      var project1 = Any.Instance<IProjectScopedRuleTarget>();
      var project2 = Any.Instance<IProjectScopedRuleTarget>();
      var project3 = Any.Instance<IProjectScopedRuleTarget>();

      ruleSet.Add(rule1);
      ruleSet.Add(rule2);
      ruleSet.Add(rule3);

      //WHEN
      ruleSet.Check(new [] { project1, project2, project3 }, report);
      
      //THEN
      rule1.Received(1).Check(project1, report);
      rule2.Received(1).Check(project1, report);
      rule3.Received(1).Check(project1, report);
      rule1.Received(1).Check(project2, report);
      rule2.Received(1).Check(project2, report);
      rule3.Received(1).Check(project2, report);
      rule1.Received(1).Check(project3, report);
      rule2.Received(1).Check(project3, report);
      rule3.Received(1).Check(project3, report);
    }
  }
}