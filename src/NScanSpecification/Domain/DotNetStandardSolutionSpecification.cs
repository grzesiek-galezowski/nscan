using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.NScan.Domain;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class DotNetStandardSolutionSpecification
  {
    [Fact]
    public void ShouldResolveReferencesOfAllProjectUsingItselfAsAContext()
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
      dotNetStandardSolution.ResolveAllProjectsReferences();

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
    public void ShouldBuildNamespacesCacheWhenAskedToBuildCache()
    {
      //GIVEN
      var namespacesCache = Substitute.For<INamespacesDependenciesCache>();
      var project1 = Substitute.For<IDotNetProject>();
      var project2 = Substitute.For<IDotNetProject>();
      var project3 = Substitute.For<IDotNetProject>();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>()
      {
        {Any.ProjectId(), project1},
        {Any.ProjectId(), project2},
        {Any.ProjectId(), project3},
      };
      var solution = new DotNetStandardSolution(projectsById, Any.Instance<IPathCache>());

      //WHEN
      solution.BuildCache();

      //THEN
      project1.Received(1).RefreshNamespacesCache();
      project2.Received(1).RefreshNamespacesCache();
      project3.Received(1).RefreshNamespacesCache();
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
    public void ShouldOrderThePathRuleSetToCheckThePathsInTheCacheForVerification()
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
    
    [Fact]
    public void ShouldOrderTheProjectScopedRuleSetToCheckTheProjectsForVerification()
    {
      //GIVEN
      var project1 = Any.Instance<IDotNetProject>();
      var project2 = Any.Instance<IDotNetProject>();
      var project3 = Any.Instance<IDotNetProject>();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>()
      {
        {Any.ProjectId(), project1},
        {Any.ProjectId(), project2},
        {Any.ProjectId(), project3},
      };
      var solution = new DotNetStandardSolution(projectsById, Any.Instance<IPathCache>());
      var ruleSet = Substitute.For<IProjectScopedRuleSet>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      solution.Check(ruleSet, report);

      //THEN
      ruleSet.Received(1).Check(ListContaining(project1, project2, project3), report);
    }

    [Fact]
    public void ShouldOrderTheNamespacesBasedRuleSetToCheckTheProjectsForVerification()
    {
      //GIVEN
      var project1 = Any.Instance<IDotNetProject>();
      var project2 = Any.Instance<IDotNetProject>();
      var project3 = Any.Instance<IDotNetProject>();
      var projectsById = new Dictionary<ProjectId, IDotNetProject>()
      {
        {Any.ProjectId(), project1},
        {Any.ProjectId(), project2},
        {Any.ProjectId(), project3},
      };
      var namespacesCache = Substitute.For<INamespacesDependenciesCache>();
      var solution = new DotNetStandardSolution(
        projectsById, 
        Any.Instance<IPathCache>());
      var ruleSet = Substitute.For<INamespacesBasedRuleSet>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      solution.Check(ruleSet, report);

      //THEN
      ruleSet.Received(1).Check(ListContaining(project1, project2, project3), report);
    }

    private static IReadOnlyList<IDotNetProject> ListContaining(IDotNetProject project1, IDotNetProject project2, IDotNetProject project3)
    {
      return Arg<IReadOnlyList<IDotNetProject>>.That(Contains(project1, project2, project3));
    }

    private static Action<IReadOnlyList<IDotNetProject>> Contains(IDotNetProject project1, IDotNetProject project2, IDotNetProject project3)
    {
      return rol => rol.Should().BeEquivalentTo(project1, project2, project3);
    }
  }
}
