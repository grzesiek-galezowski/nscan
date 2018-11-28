using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using GlobExpressions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.App;
using TddXt.NScan.Domain;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
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
      var project = new DotNetStandardProjectBuilder()
      {
        ReferencedProjectIds = referencedProjectsIds
      }.Build();
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
      var support = Substitute.For<INScanSupport>();
      var exceptionFromResolution = Any.Instance<ReferencedProjectNotFoundInSolutionException>();
      var project = new DotNetStandardProjectBuilder()
      {
        ReferencedProjectIds = referencedProjectsIds,
        Support = support
      }.Build();
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

    private static Action<ISolutionContext> ResolvingReferencesFrom(IReferencingProject project, ProjectId id2)
    {
      return s => s.ResolveReferenceFrom(project, id2);
    }

    [Fact]
    public void ShouldAddItselfWithItsIdAsAReferenceToAnotherProjectWhenAskedToResolveAsReferenceOfThisProject()
    {
      //GIVEN
      var referencingProject = Substitute.For<IReferencingProject>();
      var projectId = Any.ProjectId();
      var project = new DotNetStandardProjectBuilder()
      {
        ProjectId = projectId
      }.Build();

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
      var project = new DotNetStandardProjectBuilder()
      {
        ProjectId = projectId
      }.Build();

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
    public void ShouldSayItHasProjectIdMatchingTheOneItWasCreatedWith()
    {
      //GIVEN
      var assemblyName = Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        AssemblyName = assemblyName
      }.Build();

      //WHEN
      var hasProject = project.HasProjectAssemblyNameMatching(Pattern.WithoutExclusion(assemblyName));

      //THEN
      hasProject.Should().BeTrue();
    }

    [Fact]
    public void ShouldSayItHasProjectAssemblyNameMatchingBlobPattern()
    {
      //GIVEN
      var assemblySuffix = Any.String();
      var assemblyName = Any.String() + "." + assemblySuffix;
      var project = new DotNetStandardProjectBuilder()
      {
        AssemblyName = assemblyName
      }.Build();
      string assemblyNamePattern = "*." + assemblySuffix;

      //WHEN
      var hasProject = project.HasProjectAssemblyNameMatching(Pattern.WithoutExclusion(assemblyNamePattern));

      //THEN
      hasProject.Should().BeTrue();
    }

    [Fact]
    public void ShouldSayItDoesNotHaveMatchingProjectAssemblyNameWhenItWasNotCreatedWithMatchingOne()
    {
      //GIVEN
      var searchedAssemblyName = Any.String();
      var project = new DotNetStandardProjectBuilder
      {
        AssemblyName = Any.OtherThan(searchedAssemblyName)
      }.Build();

      //WHEN
      var hasProject = project.HasProjectAssemblyNameMatching(Pattern.WithoutExclusion(searchedAssemblyName));

      //THEN
      hasProject.Should().BeFalse();
    }


    [Fact]
    public void ShouldReturnAssemblyNameWhenAskedForStringRepresentation()
    {
      //GIVEN
      var assemblyName = Any.String();
      var project = new DotNetStandardProjectBuilder
      {
        AssemblyName = assemblyName
      }.Build();

      //WHEN
      var stringRepresentation = project.ToString();

      //THEN
      stringRepresentation.Should().Be(assemblyName);
    }

    [Fact]
    public void ShouldSayWhenItHasPackageReference()
    {
      //GIVEN
      var packageReference = Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        PackageReferences = new List<PackageReference>()
        {
          new PackageReference(packageReference, Any.String())
        }
      }.Build();

      //WHEN
      var result = project.HasPackageReferenceMatching(new Glob(packageReference));

      //THEN
      result.Should().BeTrue();
    }

    [Fact]
    public void ShouldSayWhenItDoesNotHavePackageReference()
    {
      //GIVEN
      var project = new DotNetStandardProjectBuilder().Build();

      //WHEN
      var result = project.HasPackageReferenceMatching(Any.Instance<Glob>());

      //THEN
      result.Should().BeFalse();
    }

    [Fact]
    public void ShouldSayWhenItHasAssemblyReference()
    {
      //GIVEN
      var assemblyReferenceName = Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        AssemblyReferences = new List<AssemblyReference>()
        {
          new AssemblyReference(assemblyReferenceName, Any.String())
        }
      }.Build();

      //WHEN
      var result = project.HasAssemblyReferenceWithNameMatching(new Glob(assemblyReferenceName));

      //THEN
      result.Should().BeTrue();
    }

    [Fact]
    public void ShouldSayWhenItDoesNotHaveAssemblyReferenceMatchingGivenName()
    {
      //GIVEN
      var project = new DotNetStandardProjectBuilder().Build();

      //WHEN
      var result = project.HasAssemblyReferenceWithNameMatching(Any.Instance<Glob>());

      //THEN
      result.Should().BeFalse();
    }


    [Fact]
    public void ShouldPassAllItsFilesToProjectScopedRuleAlongWithRootNamespace()
    {
      //GIVEN
      var files = Any.ReadOnlyList<ISourceCodeFile>();
      var rootNamespace = Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        RootNamespace = rootNamespace,
        Files = files
      }.Build();
      var rule = Substitute.For<IProjectScopedRule>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      project.AnalyzeFiles(rule, report);

      //THEN
      rule.Received(1).Check(files, report);
    }

    private class DotNetStandardProjectBuilder
    {
      public DotNetStandardProject Build()
      {
        return new DotNetStandardProject(
          RootNamespace,
          AssemblyName, 
          ProjectId, 
          ReferencedProjectIds, 
          PackageReferences, 
          AssemblyReferences,
          Files,
          Support);
      }

      public IReadOnlyList<PackageReference> PackageReferences { private get; set; } = Any.ReadOnlyList<PackageReference>();
      public IReadOnlyList<AssemblyReference> AssemblyReferences { private get; set; } =
        Any.ReadOnlyList<AssemblyReference>();
      public ProjectId[] ReferencedProjectIds { private get; set; } = Any.Array<ProjectId>();
      public ProjectId ProjectId { private get; set; } = Any.ProjectId();
      public string AssemblyName { private get; set; } = Any.String();
      public INScanSupport Support { private get; set; } = Any.Support();
      public string RootNamespace { get; set; } = Any.String(); //bug value object
      public IReadOnlyList<ISourceCodeFile> Files { get; set; } = Any.ReadOnlyList<ISourceCodeFile>();
    }
  }
}