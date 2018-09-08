using System;
using System.Collections.Generic;
using FluentAssertions;
using MyTool.App;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.XNSubstitute.Root;
using Xunit;

namespace MyTool
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
        { Root.Any.ProjectId(), project1 },
        { Root.Any.ProjectId(), project2 },
        { Root.Any.ProjectId(), project3 },
      };
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById, Root.Any.Instance<IPathCache>());
      

      //WHEN
      dotNetStandardSolution.ResolveAllProjectsReferences(Root.Any.Instance<IAnalysisReportInProgress>());

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
      var project2Id = Root.Any.ProjectId();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>
      {
        { Root.Any.ProjectId(), project1 },
        { project2Id, project2 },
      };
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById, Root.Any.Instance<IPathCache>());
      

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
        { Root.Any.ProjectId(), root1},
        { Root.Any.ProjectId(), nonRoot},
        { Root.Any.ProjectId(), root2}
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
      var project1Id = Root.Any.ProjectId();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>
      {
        { project1Id, project1 },
      };
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById, Root.Any.Instance<IPathCache>());


      //WHEN - THEN
      new Action(() => dotNetStandardSolution.ResolveReferenceFrom(project1, Root.Any.ProjectIdOtherThan(project1Id)))
        .Should().ThrowExactly<ReferencedProjectNotFoundInSolutionException>();
      project1.ReceivedNothing();
    }

    [Fact]
    public void ShouldOrderTheRuleSetToCheckThePathsInTheCacheForVerification()
    {
      //GIVEN
      var projectsById = Root.Any.Dictionary<ProjectId, IDotNetProject>();
      var pathCache = Root.Any.Instance<IPathCache>();
      var solution = new DotNetStandardSolution(projectsById, pathCache);
      var ruleSet = Substitute.For<IPathRuleSet>();
      var report = Root.Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      solution.Check(ruleSet, report);

      //THEN
      ruleSet.Received(1).Check(pathCache, report);
    }

  }
}
