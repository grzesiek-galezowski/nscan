using MyTool;
using MyTool.App;
using MyTool.CompositionRoot;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using Xunit;
using static TddXt.AnyRoot.Root;

public class PathRuleSetSpecification
{
  [Fact]
  public void ShouldApplyTheRuleSetToEveryPathAggregated()
  {
    //GIVEN
    //bug refactor the AddDirectIndependent...
    /*var ruleSet = new PathRuleSet();
    var report = Any.Instance<IAnalysisReportInProgress>();
    ruleSet.AddDirectIndependentOfProjectRule(depending1, dependent1);
    ruleSet.AddDirectIndependentOfProjectRule(depending2, dependent2);
    ruleSet.AddDirectIndependentOfProjectRule(depending3, dependent3);
      
    //WHEN
    var pathCache = Substitute.For<IPathCache>();
    ruleSet.Check(pathCache, report);

    //THEN
    ruleSet.Received(1).Check(path1, report);
    ruleSet.Received(1).Check(path2, report);
    ruleSet.Received(1).Check(path3, report);*/
  }
}