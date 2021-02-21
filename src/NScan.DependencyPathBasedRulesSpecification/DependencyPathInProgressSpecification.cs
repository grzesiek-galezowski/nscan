using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NScan.DependencyPathBasedRules;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.XNSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.DependencyPathBasedRulesSpecification
{
  public class DependencyPathInProgressSpecification
  {
    [Fact]
    public void ShouldAddFinalizedPathWithFinalAndClonedProjectsInOrder()
    {
      //GIVEN
      var destination = Substitute.For<IFinalDependencyPathDestination>();
      var initialProjects = Any.List<IDependencyPathBasedRuleTarget>();
      var projectDependencyPathFactory = Substitute.For<ProjectDependencyPathFactory>();
      var newDependencyPath = Any.Instance<IProjectDependencyPath>();
      var additionalProject = Any.Instance<IDependencyPathBasedRuleTarget>();
      var finalProject = Any.Instance<IDependencyPathBasedRuleTarget>();

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
