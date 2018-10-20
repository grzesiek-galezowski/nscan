using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.NScan.App;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification
{
  public class DotNetStandardSolutionSpecification
  {
    [Fact]
    public void ShouldResolveReferencesOfAllProjectUsingItselfAsAContext() //todo update this test and code to handle the report
    {
      //GIVEN
      var project1 = Substitute.For<IDotNetProject>();
      var project2 = Substitute.For<IDotNetProject>();
      var project3 = Substitute.For<IDotNetProject>();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>()
      {
        { Any.ProjectId(), project1 },
        { Any.ProjectId(), project2 },
        { Any.ProjectId(), project3 },
      };
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById, Any.Instance<IPathCache>());
      

      //WHEN
      dotNetStandardSolution.ResolveAllProjectsReferences(Any.Instance<IAnalysisReportInProgress>());

      //THEN
      Received.InOrder(() =>
      {
        project1.ResolveReferencesFrom((ISolutionContext)dotNetStandardSolution);
        project2.ResolveReferencesFrom((ISolutionContext)dotNetStandardSolution);
        project3.ResolveReferencesFrom((ISolutionContext)dotNetStandardSolution);
      });
    }

    [Fact]
    public void ShouldAddTwoWayBindingBetweenReferencedAndReferencingProjectDuringResolutionFromSolution()
    {
      //GIVEN
      var project1 = Substitute.For<IDotNetProject>();
      var project2 = Substitute.For<IDotNetProject>();
      var project2Id = Any.ProjectId();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>
      {
        { Any.ProjectId(), project1 },
        { project2Id, project2 },
      };
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById, Any.Instance<IPathCache>());
      

      //WHEN
      dotNetStandardSolution.ResolveReferenceFrom(project1, project2Id);

      //THEN
      project1.Received(1).ResolveAsReferencing(project2);
      project2.Received(1).ResolveAsReferenceOf(project1);
    }

    [Fact]
    public void ShouldBuildPathsCacheWhenAskedToBuildCache()
    {
      //GIVEN
      var root1 = Substitute.For<IDotNetProject>();
      var root2 = Substitute.For<IDotNetProject>();
      var nonRoot = Substitute.For<IDotNetProject>();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>()
      {
        { Any.ProjectId(), root1},
        { Any.ProjectId(), nonRoot},
        { Any.ProjectId(), root2}
      };
      var pathCache = Substitute.For<IPathCache>();
      var solution = new DotNetStandardSolution(projectsById, pathCache);

      root1.IsRoot().Returns(true);
      root2.IsRoot().Returns(true);
      nonRoot.IsRoot().Returns(false);

      //WHEN
      solution.BuildCache();

      //THEN
      pathCache.Received(1).BuildStartingFrom(root1, root2);
    }

    [Fact]
    public void ShouldThrowExceptionWhenResolvingReferenceToProjectWhichIsNotLoadedCorrectlyToSolution()
    {
      //GIVEN
      var project1 = Substitute.For<IDotNetProject>();
      var project1Id = Any.ProjectId();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>
      {
        { project1Id, project1 },
      };
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById, Any.Instance<IPathCache>());


      //WHEN - THEN
      new Action(() => dotNetStandardSolution.ResolveReferenceFrom(project1, Any.ProjectIdOtherThan(project1Id)))
        .Should().ThrowExactly<ReferencedProjectNotFoundInSolutionException>();
      project1.ReceivedNothing();
    }

    [Fact]
    public void ShouldOrderTheRuleSetToCheckThePathsInTheCacheForVerification()
    {
      //GIVEN
      var projectsById = Any.Dictionary<ProjectId, IDotNetProject>();
      var pathCache = Any.Instance<IPathCache>();
      var solution = new DotNetStandardSolution(projectsById, pathCache);
      var ruleSet = Substitute.For<IPathRuleSet>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      solution.Check(ruleSet, report);

      //THEN
      ruleSet.Received(1).Check(pathCache, report);
    }

  }
}
