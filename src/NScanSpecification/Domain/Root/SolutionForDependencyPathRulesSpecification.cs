using System.Collections.Generic;
using NScan.DependencyPathBasedRules;
using NScan.Domain;
using NScan.SharedKernel;
using NScanSpecification.Lib;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class SolutionForDependencyPathRulesSpecification
  {
    [Fact]
    public void ShouldOrderThePathRuleSetToCheckThePathsInTheCacheForVerification()
    {
      //GIVEN
      var projectsById = Any.ReadOnlyDictionary<ProjectId, IDotNetProject>();
      var pathCache = Any.Instance<IPathCache>();
      var solution = new SolutionForDependencyPathRules(
        pathCache, 
        projectsById);
      var ruleSet = Substitute.For<IPathRuleSet>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      solution.Check(ruleSet, report);

      //THEN
      ruleSet.Received(1).Check(pathCache, report);
    }


    [Fact]
    public void ShouldBuildPathsCacheWhenAskedToBuildCache()
    {
      //GIVEN
      var root1 = Substitute.For<IDotNetProject>();
      var root2 = Substitute.For<IDotNetProject>();
      var nonRoot = Substitute.For<IDotNetProject>();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>
      {
        { Any.ProjectId(), root1},
        { Any.ProjectId(), nonRoot},
        { Any.ProjectId(), root2}
      } as IReadOnlyDictionary<ProjectId, IDotNetProject>;
      var pathCache = Substitute.For<IPathCache>();
      var solution = new SolutionForDependencyPathRules(pathCache, 
        projectsById);

      root1.IsRoot().Returns(true);
      root2.IsRoot().Returns(true);
      nonRoot.IsRoot().Returns(false);

      //WHEN
      solution.BuildDependencyPathCache();

      //THEN
      pathCache.Received(1).BuildStartingFrom(root1, root2);
    }
  }
}