using System;
using System.Collections.Generic;
using FluentAssertions;
using NScan.DependencyPathBasedRules;
using NScan.Domain;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScanSpecification.Lib;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.XNSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
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
      var projectsById = new Dictionary<ProjectId, IDotNetProject>
      {
        { Any.ProjectId(), project1 },
        { Any.ProjectId(), project2 },
        { Any.ProjectId(), project3 },
      };
      var dotNetStandardSolution = new DotNetStandardSolution(
        projectsById, 
        Any.Instance<IPathCache>(), 
        Any.ReadOnlyList<INamespaceBasedRuleTarget>(), 
        Any.ReadOnlyList<IProjectScopedRuleTarget>());
      

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
      var dotNetStandardSolution = new DotNetStandardSolution(
        projectsById, 
        Any.Instance<IPathCache>(), 
        Any.ReadOnlyList<INamespaceBasedRuleTarget>(), 
        Any.ReadOnlyList<IProjectScopedRuleTarget>());
      

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
      var solution = new DotNetStandardSolution(
        projectsById, 
        pathCache, 
        Any.ReadOnlyList<INamespaceBasedRuleTarget>(), 
        Any.ReadOnlyList<IProjectScopedRuleTarget>());

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
      var target1 = Substitute.For<INamespaceBasedRuleTarget>();
      var target2 = Substitute.For<INamespaceBasedRuleTarget>();
      var target3 = Substitute.For<INamespaceBasedRuleTarget>();

      var projectsById = Any.ReadOnlyDictionary<ProjectId, IDotNetProject>();
      var solution = new DotNetStandardSolution(
        projectsById, 
        Any.Instance<IPathCache>(), 
        new List<INamespaceBasedRuleTarget>()
        {
          target1, target2, target3
        }, 
        Any.ReadOnlyList<IProjectScopedRuleTarget>());

      //WHEN
      solution.BuildCache();

      //THEN
      target1.Received(1).RefreshNamespacesCache();
      target2.Received(1).RefreshNamespacesCache();
      target3.Received(1).RefreshNamespacesCache();
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
      var dotNetStandardSolution = new DotNetStandardSolution(
        projectsById, 
        Any.Instance<IPathCache>(), 
        Any.ReadOnlyList<INamespaceBasedRuleTarget>(), 
        Any.ReadOnlyList<IProjectScopedRuleTarget>());


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
      var solution = new DotNetStandardSolution(
        projectsById, 
        pathCache, 
        Any.ReadOnlyList<INamespaceBasedRuleTarget>(), 
        Any.ReadOnlyList<IProjectScopedRuleTarget>());
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
      var projectScopedRuleTargets = Any.ReadOnlyList<IProjectScopedRuleTarget>();
      var solution = new DotNetStandardSolution(
        Any.Dictionary<ProjectId, IDotNetProject>(), 
        Any.Instance<IPathCache>(), 
        Any.ReadOnlyList<INamespaceBasedRuleTarget>(), 
        projectScopedRuleTargets);
      var ruleSet = Substitute.For<IProjectScopedRuleSet>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      solution.Check(ruleSet, report);

      //THEN
      ruleSet.Received(1).Check(projectScopedRuleTargets, report);
    }

    [Fact]
    public void ShouldOrderTheNamespacesBasedRuleSetToCheckTheProjectsForVerification()
    {
      //GIVEN
      var namespaceBasedRuleTargets = Any.ReadOnlyList<INamespaceBasedRuleTarget>();
      IReadOnlyDictionary<ProjectId, IDotNetProject> projectsById = Any.Dictionary<ProjectId, IDotNetProject>();
      var solution = new DotNetStandardSolution(
        projectsById,
        Any.Instance<IPathCache>(), 
        namespaceBasedRuleTargets, 
        Any.ReadOnlyList<IProjectScopedRuleTarget>());
      var ruleSet = Substitute.For<INamespacesBasedRuleSet>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      solution.Check(ruleSet, report);

      //THEN
      ruleSet.Received(1).Check(namespaceBasedRuleTargets, report);
    }
  }
}
