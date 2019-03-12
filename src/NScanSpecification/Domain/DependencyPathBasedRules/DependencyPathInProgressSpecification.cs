﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.XNSubstitute.Root;
using Xunit;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class DependencyPathInProgressSpecification
  {
    [Fact]
    public void ShouldAddFinalizedPathWithFinalAndClonedProjectsInOrder()
    {
      //GIVEN
      var destination = Substitute.For<IFinalDependencyPathDestination>();
      var initialProjects = AnyRoot.Root.Any.List<IDependencyPathBasedRuleTarget>();
      var projectDependencyPathFactory = Substitute.For<ProjectDependencyPathFactory>();
      var newDependencyPath = AnyRoot.Root.Any.Instance<IProjectDependencyPath>();
      var additionalProject = AnyRoot.Root.Any.Instance<IDependencyPathBasedRuleTarget>();
      var finalProject = AnyRoot.Root.Any.Instance<IDependencyPathBasedRuleTarget>();

      var dependencyPathInProgress = new DependencyPathInProgress(
        destination,
        projectDependencyPathFactory,
        initialProjects);

      projectDependencyPathFactory.Invoke(Concatenated(initialProjects, additionalProject, finalProject)).Returns(newDependencyPath);

      var clonedPath = dependencyPathInProgress.CloneWith(additionalProject);

      //WHEN
      clonedPath.FinalizeWith(finalProject);

      //THEN
      projectDependencyPathFactory.Received(1).Invoke(Concatenated(initialProjects, additionalProject, finalProject));
      destination.Received(1).Add(newDependencyPath);
    }

    private static IReadOnlyList<IDependencyPathBasedRuleTarget> Concatenated(
      IReadOnlyCollection<IDependencyPathBasedRuleTarget> alreadyAggregatedProjects,
      params IDependencyPathBasedRuleTarget[] additionalProjects)
    {
      return Arg<IReadOnlyList<IDependencyPathBasedRuleTarget>>.That(
        path => path.Should().BeEquivalentTo(alreadyAggregatedProjects.Concat(additionalProjects).ToList()));
    }
  }
}