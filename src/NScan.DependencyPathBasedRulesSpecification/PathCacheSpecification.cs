using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRulesSpecification;

public class PathCacheSpecification
{
  [Fact]
  public void ShouldPassNewStartingPathToEachProjectToGatherDependencies()
  {
    //GIVEN
    var dependencyPathFactory = Substitute.For<IDependencyPathFactory>();
    var pathCache = new PathCache(dependencyPathFactory);
    var project1 = Substitute.For<IDependencyPathBasedRuleTarget>();
    var project2 = Substitute.For<IDependencyPathBasedRuleTarget>();
    var project3 = Substitute.For<IDependencyPathBasedRuleTarget>();
    var dependencyStartingPath1 = Any.Instance<IDependencyPathInProgress>();
    var dependencyStartingPath2 = Any.Instance<IDependencyPathInProgress>();
    var dependencyStartingPath3 = Any.Instance<IDependencyPathInProgress>();

    dependencyPathFactory.NewDependencyPathFor(pathCache).Returns(
      dependencyStartingPath1,
      dependencyStartingPath2,
      dependencyStartingPath3);

    //WHEN
    pathCache.BuildStartingFrom([project1, project2, project3]);

    //THEN
    project1.Received(1).FillAllBranchesOf(dependencyStartingPath1);
    project2.Received(1).FillAllBranchesOf(dependencyStartingPath2);
    project3.Received(1).FillAllBranchesOf(dependencyStartingPath3);
  }

  [Fact]
  public void ShouldMakePassedRuleCheckAllItsPaths()
  {
    //GIVEN
    var pathCache = new PathCache(Any.Instance<IDependencyPathFactory>());
    var rule = Substitute.For<IDependencyRule>();
    var report = Any.Instance<IAnalysisReportInProgress>();
    var path1 = Any.Instance<IProjectDependencyPath>();
    var path2 = Any.Instance<IProjectDependencyPath>();
    var path3 = Any.Instance<IProjectDependencyPath>();
    pathCache.Add(path1);
    pathCache.Add(path2);
    pathCache.Add(path3);

    //WHEN
    pathCache.Check(rule, report);
      
    //THEN
    rule.Received(1).Check(report, path1);
    rule.Received(1).Check(report, path2);
    rule.Received(1).Check(report, path3);
  }
}
