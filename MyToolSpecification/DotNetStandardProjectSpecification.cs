using System;
using FluentAssertions;
using MyTool.App;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using Xunit;

namespace MyTool
{
  public class DotNetStandardProjectSpecification
  {
    [Fact]
    public void ShouldTellSolutionToResolveAllItsReferencesByIds()
    {
      //GIVEN
      var id1 = Root.Any.ProjectId();
      var id2 = Root.Any.ProjectId();
      var id3 = Root.Any.ProjectId();
      var referencedProjectsIds = new[] { id1, id2, id3 };
      var project = new DotNetStandardProject(Root.Any.String(), Root.Any.ProjectId(), referencedProjectsIds, Root.Any.Support());
      var solution = Substitute.For<ISolutionContext>();

      //WHEN
      project.ResolveReferencesFrom(solution);

      //THEN
      solution.Received(1).ResolveReferenceFrom(project, id1);
      solution.Received(1).ResolveReferenceFrom(project, id2);
      solution.Received(1).ResolveReferenceFrom(project, id3);
    }

    [Fact]
    public void ShouldLogErrorAndIgnoreProjectThatCannotBeResolved()
    {
      //GIVEN
      var id1 = Root.Any.ProjectId();
      var id2 = Root.Any.ProjectId();
      var id3 = Root.Any.ProjectId();
      var referencedProjectsIds = new[] { id1, id2, id3 };
      var support = Substitute.For<ISupport>();
      var exceptionFromResolution = Root.Any.Instance<ReferencedProjectNotFoundInSolutionException>();
      var project = new DotNetStandardProject(Root.Any.String(), Root.Any.ProjectId(), referencedProjectsIds, support);
      var solution = Substitute.For<ISolutionContext>();

      solution.When(ResolvingReferencesFrom(project, id2)).Throw(exceptionFromResolution);


      //WHEN
      project.ResolveReferencesFrom(solution);

      //THEN
      solution.Received(1).ResolveReferenceFrom(project, id1);
      solution.Received(1).ResolveReferenceFrom(project, id2);
      solution.Received(1).ResolveReferenceFrom(project, id3);
      support.Received(1).Report(exceptionFromResolution);

    }

    private static Action<ISolutionContext> ResolvingReferencesFrom(DotNetStandardProject project, ProjectId id2)
    {
      return s => s.ResolveReferenceFrom(project, id2);
    }

    [Fact]
    public void ShouldAddItselfWithItsIdAsAReferenceToAnotherProjectWhenAskedToResolveAsReferenceOfThisProject()
    {
      //GIVEN
      var referencingProject = Substitute.For<IReferencingProject>();
      var projectId = Root.Any.ProjectId();
      var project = new DotNetStandardProject(Root.Any.String(), projectId, Root.Any.Array<ProjectId>(), Root.Any.Support());

      //WHEN
      project.ResolveAsReferenceOf(referencingProject);

      //THEN
      referencingProject.Received(1).AddReferencedProject(projectId, project);

    }

    [Fact]
    public void ShouldAddItselfWithItsIdAsAReferencingProjectToAnotherProjectWhenAskedToResolveAsReferencingThisProject()
    {
      //GIVEN
      var referencedProject = Substitute.For<IReferencedProject>();
      var projectId = Root.Any.ProjectId();
      var project = new DotNetStandardProject(Root.Any.String(), projectId, Root.Any.Array<ProjectId>(), Root.Any.Support());

      //WHEN
      project.ResolveAsReferencing(referencedProject);

      //THEN
      referencedProject.Received(1).AddReferencingProject(projectId, project);
    }

    [Fact]
    public void ShouldSayItIsARootWhenItHasNoReferencingProjects()
    {
      //GIVEN
      var project = new DotNetStandardProjectBuilder().Build();

      //WHEN
      var isRoot = project.IsRoot();

      //THEN
      isRoot.Should().BeTrue();
    }

    [Fact]
    public void ShouldSayItIsNotARootWhenItHasAtLeastOneReferencingProject()
    {
      //GIVEN
      var project = new DotNetStandardProjectBuilder().Build();
      project.AddReferencingProject(Root.Any.ProjectId(), Root.Any.Instance<IReferencingProject>());

      //WHEN
      var isRoot = project.IsRoot();

      //THEN
      isRoot.Should().BeFalse();
    }

    [Fact]
    public void ShouldAddItselfToPathThenFillCopyForEachReferencedProjects()
    {
      //GIVEN
      var project = new DotNetStandardProjectBuilder().Build();
      var reference1 = Substitute.For<IReferencedProject>();
      var reference2 = Substitute.For<IReferencedProject>();
      var reference3 = Substitute.For<IReferencedProject>();
      var dependencyPathInProgress = Substitute.For<IDependencyPathInProgress>();
      var clonedPathInProgress1 = Root.Any.Instance<IDependencyPathInProgress>();
      var clonedPathInProgress2 = Root.Any.Instance<IDependencyPathInProgress>();
      var clonedPathInProgress3 = Root.Any.Instance<IDependencyPathInProgress>();

      project.AddReferencedProject(Root.Any.ProjectId(), reference1);
      project.AddReferencedProject(Root.Any.ProjectId(), reference2);
      project.AddReferencedProject(Root.Any.ProjectId(), reference3);

      dependencyPathInProgress.CloneWith(project).Returns(
        clonedPathInProgress1,
        clonedPathInProgress2,
        clonedPathInProgress3);

      //WHEN
      project.FillAllBranchesOf(dependencyPathInProgress);

      //THEN
      reference1.Received(1).FillAllBranchesOf(clonedPathInProgress1);
      reference2.Received(1).FillAllBranchesOf(clonedPathInProgress2);
      reference3.Received(1).FillAllBranchesOf(clonedPathInProgress3);
    }

    [Fact]
    public void ShouldFinalizePathWithItselfWhenItDoesNotHaveAnyReferences()
    {
      //GIVEN
      var project = new DotNetStandardProjectBuilder().Build();
      var dependencyPathInProgress = Substitute.For<IDependencyPathInProgress>();

      //WHEN
      project.FillAllBranchesOf(dependencyPathInProgress);

      //THEN
      dependencyPathInProgress.Received(1).FinalizeWith(project);
    }

    private class DotNetStandardProjectBuilder
    {
      public DotNetStandardProject Build()
      {
        return new DotNetStandardProject(AssemblyName, ProjectId, ReferencedProjectIds, Support);
      }

      public ProjectId[] ReferencedProjectIds { private get; set; } = Root.Any.Array<ProjectId>();

      public ProjectId ProjectId { private get; set; } = Root.Any.ProjectId();

      public string AssemblyName { private get; set; } = Root.Any.String();

      public ISupport Support { private get; set; } = Root.Any.Support();
    }
  }
}