using System;
using System.Collections.Generic;
using FluentAssertions;
using MyTool;
using MyTool.App;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyToolSpecification
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
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById);
      

      //WHEN
      dotNetStandardSolution.ResolveAllProjectsReferences(Any.Instance<IAnalysisInProgressReport>());

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
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById);
      

      //WHEN
      dotNetStandardSolution.ResolveReferenceFrom(project1, project2Id);

      //THEN
      project1.Received(1).ResolveAsReferencing(project2);
      project2.Received(1).ResolveAsReferenceOf(project1);
    }

    [Fact]
    public void ShouldBuildPathsCacheWhenAskedToBuildCaches()
    {
      //GIVEN
      var projectsById = Any.Dictionary<ProjectId, IDotNetProject>();
      var root1 = Substitute.For<IDotNetProject>();
      var root2 = Substitute.For<IDotNetProject>();
      var nonRoot = Substitute.For<IDotNetProject>();
      var pathCache = Substitute.For<IPathCache>();
      var solution = new DotNetStandardSolution(projectsById, pathCache);


      root1.IsRoot().Returns(true);
      root2.IsRoot().Returns(true);
      nonRoot.IsRoot().Returns(false);

      //WHEN
      solution.BuildCaches();

      //THEN
      pathCache.BuildStartingFrom(root1, root2);
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
      var dotNetStandardSolution = new DotNetStandardSolution(projectsById);


      //WHEN - THEN
      new Action(() => dotNetStandardSolution.ResolveReferenceFrom(project1, Any.ProjectIdOtherThan(project1Id)))
        .Should().ThrowExactly<ReferencedProjectNotFoundInSolutionException>();
      project1.ReceivedNothing();
    }

    
  }
}
