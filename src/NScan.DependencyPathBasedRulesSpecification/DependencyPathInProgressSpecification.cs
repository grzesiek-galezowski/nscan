using LanguageExt;
using NScan.DependencyPathBasedRules;
using NScanSpecification.Lib;

namespace NScan.DependencyPathBasedRulesSpecification;

public class DependencyPathInProgressSpecification
{
  [Fact]
  public void ShouldAddFinalizedPathWithFinalAndClonedProjectsInOrder()
  {
    //GIVEN
    var destination = Substitute.For<IFinalDependencyPathDestination>();
    var initialProjects = Any.Seq<IDependencyPathBasedRuleTarget>();
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

  private static Seq<IDependencyPathBasedRuleTarget> Concatenated(
    Seq<IDependencyPathBasedRuleTarget> alreadyAggregatedProjects,
    params IDependencyPathBasedRuleTarget[] additionalProjects)
  {
    return Arg<Seq<IDependencyPathBasedRuleTarget>>.That(
      path => path.Should().BeEquivalentTo(alreadyAggregatedProjects.Concat(additionalProjects).ToList()));
  }
}
