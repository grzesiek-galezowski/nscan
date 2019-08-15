using NScan.Domain.Domain.DependencyPathBasedRules;
using NScan.SharedKernel.SharedKernel;
using NSubstitute;
using Xunit;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class PathRuleSetSpecification
  {
    [Fact]
    public void ShouldPassEachRuleToCacheForVerification()
    {
      //GIVEN
      var ruleSet = new PathRuleSet();
      var report = AnyRoot.Root.Any.Instance<IAnalysisReportInProgress>();
      var pathCache = Substitute.For<IPathCache>();
      var rule1 = AnyRoot.Root.Any.Instance<IDependencyRule>();
      var rule2 = AnyRoot.Root.Any.Instance<IDependencyRule>();
      var rule3 = AnyRoot.Root.Any.Instance<IDependencyRule>();
      ruleSet.Add(rule1);
      ruleSet.Add(rule2);
      ruleSet.Add(rule3);
      
      //WHEN
      ruleSet.Check(pathCache, report);

      //THEN
      pathCache.Received(1).Check(rule1, report);
      pathCache.Received(1).Check(rule2, report);
      pathCache.Received(1).Check(rule3, report);
    }
  }
}