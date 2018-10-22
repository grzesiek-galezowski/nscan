﻿using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification
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
      var dependencyStartingPath1 = Any.Instance<IDependencyPathInProgress>();
      var dependencyStartingPath2 = Any.Instance<IDependencyPathInProgress>();
      var dependencyStartingPath3 = Any.Instance<IDependencyPathInProgress>();

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
      var pathCache = new PathCache(Any.Instance<IDependencyPathFactory>());
      var rule = Substitute.For<IDependencyRule>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      var path1 = Any.ReadOnlyList<IReferencedProject>();
      var path2 = Any.ReadOnlyList<IReferencedProject>();
      var path3 = Any.ReadOnlyList<IReferencedProject>();

      pathCache.Add(path1);
      pathCache.Add(path2);
      pathCache.Add(path3);

      //WHEN
      pathCache.Check(rule, report);
      
      //THEN
      rule.Received(1).Check(path1, report);
      rule.Received(1).Check(path2, report);
      rule.Received(1).Check(path3, report);
    }
  }
}