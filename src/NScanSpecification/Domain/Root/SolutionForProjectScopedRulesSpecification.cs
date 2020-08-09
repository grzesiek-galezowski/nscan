using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class SolutionForProjectScopedRulesSpecification
  {
    [Fact]
    public void ShouldOrderTheProjectScopedRuleSetToCheckTheProjectsForVerification()
    {
      //GIVEN
      var projectScopedRuleTargets = Any.ReadOnlyList<IProjectScopedRuleTarget>();
      var solution = new SolutionForProjectScopedRules(projectScopedRuleTargets);
      var ruleSet = Substitute.For<IProjectScopedRuleSet>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      solution.Check(ruleSet, report);

      //THEN
      ruleSet.Received(1).Check(projectScopedRuleTargets, report);
    }
  }
}