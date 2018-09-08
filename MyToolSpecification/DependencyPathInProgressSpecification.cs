using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MyTool.App;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.XNSubstitute.Root;
using Xunit;

namespace MyTool
{
  public class DependencyPathInProgressSpecification
  {
    [Fact]
    public void ShouldAddFinalizedPathWithFinalAndCloningProjectsInOrder()
    {
      //GIVEN
      var destination = Substitute.For<IFinalDependencyPathDestination>();
      var alreadyAggregatedProjects = Root.Any.List<IReferencedProject>();
      var dependencyPathInProgress = new DependencyPathInProgress(destination, alreadyAggregatedProjects);
      var additionalProject = Root.Any.Instance<IReferencedProject>();
      var finalProject = Root.Any.Instance<IReferencedProject>();
      var clonedPath = dependencyPathInProgress.CloneWith(additionalProject);
      
      //WHEN
      clonedPath.FinalizeWith(finalProject);

      //THEN
      destination.Received(1).Add(Concatenated(alreadyAggregatedProjects, additionalProject, finalProject));
    }

    private static IReadOnlyList<IReferencedProject> Concatenated(
      List<IReferencedProject> alreadyAggregatedProjects, 
      params IReferencedProject[] additionalProjects)
    {
      return Arg<IReadOnlyList<IReferencedProject>>.That(
        path => path.Should().BeEquivalentTo(alreadyAggregatedProjects.Concat(additionalProjects).ToList()));
    }
  }
}
