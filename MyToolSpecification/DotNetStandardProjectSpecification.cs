using System;
using System.Linq;
using FluentAssertions;
using MyTool.App;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyToolSpecification
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
    public void ShouldXXXXXXXXXXXXXXXXXXX() //todo
    {
      //GIVEN
      
      

      //WHEN

      //THEN
    }

    private class DotNetStandardProjectBuilder
    {
      public DotNetStandardProject Build()
      {
        return new DotNetStandardProject(this.AssemblyName, this.ProjectId, this.ReferencedProjectIds, this.Support);
      }

      public ProjectId[] ReferencedProjectIds { get; } = Any.Array<ProjectId>();

      public ProjectId ProjectId { get; } = Any.ProjectId();

      public string AssemblyName { get; } = Any.String();

      public ISupport Support { get; } = Any.Support();
    }
  }
}