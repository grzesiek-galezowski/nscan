using NSubstitute;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.Domain.SharedKernel;
using Xunit;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class PathCacheSpecification
  {
    [Fact]
    public void ShouldPassNewStartingPathToEachProjectToGatherDependencies()
    {
      //GIVEN
      var dependencyPathFactory = Substitute.For<IDependencyPathFactory>();
      var pathCache = new PathCache(dependencyPathFactory);
      var project1 = Substitute.For<IDotNetProject>();
      var project2 = Substitute.For<IDotNetProject>();
      var project3 = Substitute.For<IDotNetProject>();
      var dependencyStartingPath1 = AnyRoot.Root.Any.Instance<IDependencyPathInProgress>();
      var dependencyStartingPath2 = AnyRoot.Root.Any.Instance<IDependencyPathInProgress>();
      var dependencyStartingPath3 = AnyRoot.Root.Any.Instance<IDependencyPathInProgress>();

      dependencyPathFactory.NewDependencyPathFor((IFinalDependencyPathDestination)pathCache).Returns(
        dependencyStartingPath1,
        dependencyStartingPath2,
        dependencyStartingPath3);

      //WHEN
      pathCache.BuildStartingFrom(project1, project2, project3);

      //THEN
      project1.Received(1).FillAllBranchesOf(dependencyStartingPath1);
      project2.Received(1).FillAllBranchesOf(dependencyStartingPath2);
      project3.Received(1).FillAllBranchesOf(dependencyStartingPath3);
    }

    [Fact]
    public void ShouldMakePassedRuleCheckAllItsPaths()
    {
      //GIVEN
      var pathCache = new PathCache(AnyRoot.Root.Any.Instance<IDependencyPathFactory>());
      var rule = Substitute.For<IDependencyRule>();
      var report = AnyRoot.Root.Any.Instance<IAnalysisReportInProgress>();
      var path1 = AnyRoot.Root.Any.Instance<IProjectDependencyPath>();
      var path2 = AnyRoot.Root.Any.Instance<IProjectDependencyPath>();
      var path3 = AnyRoot.Root.Any.Instance<IProjectDependencyPath>();
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
}
