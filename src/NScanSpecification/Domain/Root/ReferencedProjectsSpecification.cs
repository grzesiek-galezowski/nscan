using System;
using NScan.DependencyPathBasedRules;
using NScan.Domain;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScanSpecification.Lib;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class ReferencedProjectsSpecification
  {
    [Fact]
    public void ShouldAddProjectToPathThenFillCopyForEachReferencedProjects()
    {
      //GIVEN
      var projects = new ReferencedProjects(Any.Array<ProjectId>(), Any.Instance<INScanSupport>());
      var reference1 = Substitute.For<IReferencedProject>();
      var reference2 = Substitute.For<IReferencedProject>();
      var reference3 = Substitute.For<IReferencedProject>();
      var dependencyPathInProgress = Substitute.For<IDependencyPathInProgress>();
      var clonedPathInProgress1 = Any.Instance<IDependencyPathInProgress>();
      var clonedPathInProgress2 = Any.Instance<IDependencyPathInProgress>();
      var clonedPathInProgress3 = Any.Instance<IDependencyPathInProgress>();
      var project = Any.Instance<IDependencyPathBasedRuleTarget>();


      projects.Add(Any.ProjectId(), reference1);
      projects.Add(Any.ProjectId(), reference2);
      projects.Add(Any.ProjectId(), reference3);

      dependencyPathInProgress.CloneWith(project).Returns(
        clonedPathInProgress1,
        clonedPathInProgress2,
        clonedPathInProgress3);

      //WHEN
      projects.FillAllBranchesOf(dependencyPathInProgress, project);

      //THEN
      reference1.Received(1).FillAllBranchesOf(clonedPathInProgress1);
      reference2.Received(1).FillAllBranchesOf(clonedPathInProgress2);
      reference3.Received(1).FillAllBranchesOf(clonedPathInProgress3);
    }
    
    [Fact]
    public void ShouldFinalizeDependencyPathWithProjectWhenTheReferencesAreEmpty()
    {
      //GIVEN
      var projects = new ReferencedProjects(Any.Array<ProjectId>(), Any.Instance<INScanSupport>());
      var dependencyPathInProgress = Substitute.For<IDependencyPathInProgress>();
      var project = Any.Instance<IDependencyPathBasedRuleTarget>();

      //WHEN
      projects.FillAllBranchesOf(dependencyPathInProgress, project);

      //THEN
      dependencyPathInProgress.Received(1).FinalizeWith(project);
    }

    [Fact]
    public void ShouldResolveProjectsAndLogAllErrorsThatOccur()
    {
      //GIVEN
      var id1 = Any.ProjectId();
      var id2 = Any.ProjectId();
      var id3 = Any.ProjectId();
      var referencedProjectsIds = new[] { id1, id2, id3 };
      var support = Substitute.For<INScanSupport>();
      var exceptionFromResolution = Any.Instance<ReferencedProjectNotFoundInSolutionException>();
      var projects = new ReferencedProjects(referencedProjectsIds, support);
      var solution = Substitute.For<ISolutionContext>();
      var project = Any.Instance<IReferencingProject>();

      solution.When(ResolvingReferencesFrom(project, id2)).Throw(exceptionFromResolution);

      //WHEN
      projects.ResolveFrom(project, solution);

      //THEN
      solution.Received(1).ResolveReferenceFrom(project, id1);
      solution.Received(1).ResolveReferenceFrom(project, id2);
      solution.Received(1).ResolveReferenceFrom(project, id3);
      support.Received(1).Report(exceptionFromResolution);
    }

    private static Action<ISolutionContext> ResolvingReferencesFrom(IReferencingProject project, ProjectId id2)
    {
      return s => s.ResolveReferenceFrom(project, id2);
    }
  }
}