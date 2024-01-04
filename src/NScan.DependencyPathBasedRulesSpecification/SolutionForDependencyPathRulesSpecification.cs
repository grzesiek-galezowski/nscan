using System;
using LanguageExt;
using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;
using NScanSpecification.Lib;

namespace NScan.DependencyPathBasedRulesSpecification;

public class SolutionForDependencyPathRulesSpecification
{
  [Fact]
  public void ShouldResolveReferencesOfAllProjectUsingItselfAsAContext()
  {
    //GIVEN
    var project1 = Substitute.For<IDotNetProject>();
    var project2 = Substitute.For<IDotNetProject>();
    var project3 = Substitute.For<IDotNetProject>();
    var projectsById = HashMap.create(
      (Any.ProjectId(), project1),
      (Any.ProjectId(), project2),
      (Any.ProjectId(), project3)
    );
    var dotNetStandardSolution = new SolutionForDependencyPathRules(
      Any.Instance<IPathCache>(), projectsById);

    //WHEN
    dotNetStandardSolution.ResolveAllProjectsReferences();

    //THEN
    project1.Received(1).ResolveReferencesFrom((ISolutionContext)dotNetStandardSolution);
    project2.Received(1).ResolveReferencesFrom((ISolutionContext)dotNetStandardSolution);
    project3.Received(1).ResolveReferencesFrom((ISolutionContext)dotNetStandardSolution);
  }

  [Fact]
  public void ShouldAddTwoWayBindingBetweenReferencedAndReferencingProjectDuringResolutionFromSolution()
  {
    //GIVEN
    var project1 = Substitute.For<IDotNetProject>();
    var project2 = Substitute.For<IDotNetProject>();
    var project2Id = Any.ProjectId();
    var projectsById = HashMap.create(
      (Any.ProjectId(), project1),
      (project2Id, project2)
    );
    var dotNetStandardSolution = new SolutionForDependencyPathRules(
      Any.Instance<IPathCache>(), projectsById);
      
    //WHEN
    dotNetStandardSolution.ResolveReferenceFrom(project1, project2Id);

    //THEN
    project1.Received(1).ResolveAsReferencing(project2);
    project2.Received(1).ResolveAsReferenceOf(project1);
  }
  
  [Fact]
  public void ShouldThrowExceptionWhenResolvingReferenceToProjectWhichIsNotLoadedCorrectlyToSolution()
  {
    //GIVEN
    var project1 = Substitute.For<IDotNetProject>();
    var project1Id = Any.ProjectId();
    var projectsById = HashMap.create((project1Id, project1));
    var dotNetStandardSolution = new SolutionForDependencyPathRules(
      Any.Instance<IPathCache>(), projectsById);

    //WHEN - THEN
    new Action(() => dotNetStandardSolution.ResolveReferenceFrom(project1, Any.ProjectIdOtherThan(project1Id)))
      .Should().ThrowExactly<ReferencedProjectNotFoundInSolutionException>();
    project1.ReceivedNothing();
  }

  [Fact]
  public void ShouldOrderThePathRuleSetToCheckThePathsInTheCacheForVerification()
  {
    //GIVEN
    var projectsById = Any.HashMap<ProjectId, IDotNetProject>();
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
    var root1Id = Any.ProjectId();
    var root2Id = Any.ProjectId();
    var projectsById = HashMap.create(
      (root1Id, root1),
      (Any.ProjectId(), nonRoot),
      (root2Id, root2)
    );
    var pathCache = Substitute.For<IPathCache>();
    var solution = new SolutionForDependencyPathRules(pathCache, 
      projectsById);

    root1.IsRoot().Returns(true);
    root2.IsRoot().Returns(true);
    nonRoot.IsRoot().Returns(false);

    //WHEN
    solution.BuildDependencyPathCache();

    //THEN
    pathCache.Received(1).BuildStartingFrom(HashMap.create((root1Id, root1), (root2Id, root2)).Values.ToSeq());
  }
}
