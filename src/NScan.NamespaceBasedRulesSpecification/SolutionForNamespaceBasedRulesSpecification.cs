using NScan.NamespaceBasedRules;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRulesSpecification;

public class SolutionForNamespaceBasedRulesSpecification
{
  [Fact]
  public void ShouldBuildNamespacesCacheWhenAskedToBuildCache()
  {
    //GIVEN
    var target1 = Substitute.For<INamespaceBasedRuleTarget>();
    var target2 = Substitute.For<INamespaceBasedRuleTarget>();
    var target3 = Substitute.For<INamespaceBasedRuleTarget>();

    var solution = new SolutionForNamespaceBasedRules(
      new List<INamespaceBasedRuleTarget>
      {
        target1, target2, target3
      });

    //WHEN
    solution.BuildNamespacesCache();

    //THEN
    target1.Received(1).RefreshNamespacesCache();
    target2.Received(1).RefreshNamespacesCache();
    target3.Received(1).RefreshNamespacesCache();
  }

  [Fact]
  public void ShouldOrderTheNamespacesBasedRuleSetToCheckTheProjectsForVerification()
  {
    //GIVEN
    var namespaceBasedRuleTargets = Any.ReadOnlyList<INamespaceBasedRuleTarget>();
    var solution = new SolutionForNamespaceBasedRules(
      namespaceBasedRuleTargets);
    var ruleSet = Substitute.For<INamespacesBasedRuleSet>();
    var report = Any.Instance<IAnalysisReportInProgress>();
      
    //WHEN
    solution.Check(ruleSet, report);

    //THEN
    ruleSet.Received(1).Check(namespaceBasedRuleTargets, report);
  }
}