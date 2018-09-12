using System;
using FluentAssertions;
using MyTool.App;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyTool
{
  public class DotNetStandardProjectSpecification
  {
    [Fact]
    public void ShouldTellSolutionToResolveAllItsReferencesByIds()
    {
      //GIVEN
      var id1 = Any.ProjectId();
      var id2 = Any.ProjectId();
      var id3 = Any.ProjectId();
      var referencedProjectsIds = new[] { id1, id2, id3 };
      var project = new DotNetStandardProject(Any.String(), Any.ProjectId(), referencedProjectsIds, Any.Support());
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
      var id1 = Any.ProjectId();
      var id2 = Any.ProjectId();
      var id3 = Any.ProjectId();
      var referencedProjectsIds = new[] { id1, id2, id3 };
      var support = Substitute.For<ISupport>();
      var exceptionFromResolution = Any.Instance<ReferencedProjectNotFoundInSolutionException>();
      var project = new DotNetStandardProject(Any.String(), Any.ProjectId(), referencedProjectsIds, support);
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
      var projectId = Any.ProjectId();
      var project = new DotNetStandardProject(Any.String(), projectId, Any.Array<ProjectId>(), Any.Support());

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
      var projectId = Any.ProjectId();
      var project = new DotNetStandardProject(Any.String(), projectId, Any.Array<ProjectId>(), Any.Support());

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
      project.AddReferencingProject(Any.ProjectId(), Any.Instance<IReferencingProject>());

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
      var clonedPathInProgress1 = Any.Instance<IDependencyPathInProgress>();
      var clonedPathInProgress2 = Any.Instance<IDependencyPathInProgress>();
      var clonedPathInProgress3 = Any.Instance<IDependencyPathInProgress>();

      project.AddReferencedProject(Any.ProjectId(), reference1);
      project.AddReferencedProject(Any.ProjectId(), reference2);
      project.AddReferencedProject(Any.ProjectId(), reference3);

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


    [Fact]
    public void ShouldSayItHasProjectIdWhenItWasCreatedWithIt()
    {
      //GIVEN
      var assemblyName = Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        AssemblyName = assemblyName
      }.Build();

      //WHEN
      var hasProject = project.HasAssemblyName(assemblyName);

      //THEN
      hasProject.Should().BeTrue();
    }

    [Fact]
    public void ShouldSayItDoesNotHaveProjectIdWhenItWasNotCreatedWithIt()
    {
      //GIVEN
      var assemblyName = Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        AssemblyName = Any.OtherThan(assemblyName)
      }.Build();

      //WHEN
      var hasProject = project.HasAssemblyName(assemblyName);

      //THEN
      hasProject.Should().BeFalse();
    }


    [Fact]
    public void ShouldReturnAssemblyNameWhenAskedForStringRepresentation()
    {
      //GIVEN
      var assemblyName = Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        AssemblyName = assemblyName
      }.Build();

      //WHEN
      var stringRepresentation = project.ToString();

      //THEN
      stringRepresentation.Should().Be(assemblyName);
    }

    private class DotNetStandardProjectBuilder
    {
      public DotNetStandardProject Build()
      {
        return new DotNetStandardProject(AssemblyName, ProjectId, ReferencedProjectIds, Support);
      }

      public ProjectId[] ReferencedProjectIds { private get; set; } = Any.Array<ProjectId>();

      public ProjectId ProjectId { private get; set; } = Any.ProjectId();

      public string AssemblyName { private get; set; } = Any.String();

      public ISupport Support { private get; set; } = Any.Support();
    }
  }
}