using MyTool.CompositionRoot;
using NSubstitute;
using TddXt.AnyRoot;
using Xunit;

namespace MyTool
{
  public class PathRuleSetSpecification
  {
    [Fact]
    public void ShouldPassEachRuleToCacheForVerification()
    {
      //GIVEN
      var ruleSet = new PathRuleSet();
      var report = Root.Any.Instance<IAnalysisReportInProgress>();
      var pathCache = Substitute.For<IPathCache>();
      var rule1 = Root.Any.Instance<IDependencyRule>();
      var rule2 = Root.Any.Instance<IDependencyRule>();
      var rule3 = Root.Any.Instance<IDependencyRule>();
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