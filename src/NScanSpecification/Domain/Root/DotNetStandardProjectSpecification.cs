using System;
using System.Collections.Generic;
using FluentAssertions;
using GlobExpressions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules.Ports;
using Xunit;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class DotNetStandardProjectSpecification
  {
    [Fact]
    public void ShouldTellSolutionToResolveAllItsReferencesByIds()
    {
      //GIVEN
      var id1 = AnyRoot.Root.Any.ProjectId();
      var id2 = AnyRoot.Root.Any.ProjectId();
      var id3 = AnyRoot.Root.Any.ProjectId();
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
      var id1 = AnyRoot.Root.Any.ProjectId();
      var id2 = AnyRoot.Root.Any.ProjectId();
      var id3 = AnyRoot.Root.Any.ProjectId();
      var referencedProjectsIds = new[] { id1, id2, id3 };
      var support = Substitute.For<INScanSupport>();
      var exceptionFromResolution = AnyRoot.Root.Any.Instance<ReferencedProjectNotFoundInSolutionException>();
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
      var projectId = AnyRoot.Root.Any.ProjectId();
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
      var projectId = AnyRoot.Root.Any.ProjectId();
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
      project.AddReferencingProject(AnyRoot.Root.Any.ProjectId(), AnyRoot.Root.Any.Instance<IReferencingProject>());

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
      var clonedPathInProgress1 = AnyRoot.Root.Any.Instance<IDependencyPathInProgress>();
      var clonedPathInProgress2 = AnyRoot.Root.Any.Instance<IDependencyPathInProgress>();
      var clonedPathInProgress3 = AnyRoot.Root.Any.Instance<IDependencyPathInProgress>();

      project.AddReferencedProject(AnyRoot.Root.Any.ProjectId(), reference1);
      project.AddReferencedProject(AnyRoot.Root.Any.ProjectId(), reference2);
      project.AddReferencedProject(AnyRoot.Root.Any.ProjectId(), reference3);

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
      var assemblyName = AnyRoot.Root.Any.String();
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
      var assemblySuffix = AnyRoot.Root.Any.String();
      var assemblyName = AnyRoot.Root.Any.String() + "." + assemblySuffix;
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
      var searchedAssemblyName = AnyRoot.Root.Any.String();
      var project = new DotNetStandardProjectBuilder
      {
        AssemblyName = AnyRoot.Root.Any.OtherThan(searchedAssemblyName)
      }.Build();

      //WHEN
      var hasProject = project.HasProjectAssemblyNameMatching(Pattern.WithoutExclusion(searchedAssemblyName));

      //THEN
      hasProject.Should().BeFalse();
    }

    [Fact]
    public void ShouldAddAllFilesInfoToNamespacesCacheWhenAskedToRefreshIt()
    {
      //GIVEN
      var file1 = Substitute.For<ISourceCodeFile>();
      var file2 = Substitute.For<ISourceCodeFile>();
      var file3 = Substitute.For<ISourceCodeFile>();
      var files = new List<ISourceCodeFile>()
      {
        file1, file2, file3
      };
      var namespacesCache = AnyRoot.Root.Any.Instance<INamespacesDependenciesCache>();
      var project = new DotNetStandardProjectBuilder
      {
        NamespacesDependenciesCache = namespacesCache,
        Files = files
      }.Build();

      //WHEN
      project.RefreshNamespacesCache();

      //THEN
      Received.InOrder(() =>
      {
        file1.AddNamespaceMappingTo(namespacesCache);
        file2.AddNamespaceMappingTo(namespacesCache);
        file3.AddNamespaceMappingTo(namespacesCache);
      });
    }

    [Fact]
    public void ShouldEvaluateRuleWithItsNamespaceDependenciesMapping()
    {
      //GIVEN
      var namespacesCache = AnyRoot.Root.Any.Instance<INamespacesDependenciesCache>();
      var rule = Substitute.For<INamespacesBasedRule>();
      var report = AnyRoot.Root.Any.Instance<IAnalysisReportInProgress>();
      var projectAssemblyName = AnyRoot.Root.Any.String();
      var project = new DotNetStandardProjectBuilder
      {
        NamespacesDependenciesCache = namespacesCache,
        AssemblyName = projectAssemblyName
      }.Build();

      //WHEN
      project.Evaluate(rule, report);

      //THEN
      rule.Received(1).Evaluate(projectAssemblyName, namespacesCache, report);
    }


    [Fact]
    public void ShouldReturnAssemblyNameWhenAskedForStringRepresentation()
    {
      //GIVEN
      var assemblyName = AnyRoot.Root.Any.String();
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
      var packageReference = AnyRoot.Root.Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        PackageReferences = new List<PackageReference>()
        {
          new PackageReference(packageReference, AnyRoot.Root.Any.String())
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
      var result = project.HasPackageReferenceMatching(AnyRoot.Root.Any.Instance<Glob>());

      //THEN
      result.Should().BeFalse();
    }

    [Fact]
    public void ShouldSayWhenItHasAssemblyReference()
    {
      //GIVEN
      var assemblyReferenceName = AnyRoot.Root.Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        AssemblyReferences = new List<AssemblyReference>()
        {
          new AssemblyReference(assemblyReferenceName, AnyRoot.Root.Any.String())
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
      var result = project.HasAssemblyReferenceWithNameMatching(AnyRoot.Root.Any.Instance<Glob>());

      //THEN
      result.Should().BeFalse();
    }


    [Fact]
    public void ShouldPassAllItsFilesToProjectScopedRuleAlongWithRootNamespace()
    {
      //GIVEN
      var files = AnyRoot.Root.Any.ReadOnlyList<ISourceCodeFile>();
      var rootNamespace = AnyRoot.Root.Any.String();
      var project = new DotNetStandardProjectBuilder()
      {
        RootNamespace = rootNamespace,
        Files = files
      }.Build();
      var rule = Substitute.For<IProjectScopedRule>();
      var report = AnyRoot.Root.Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      project.AnalyzeFiles(rule, report);

      //THEN
      rule.Received(1).Check(files, report);
    }

    [Theory]
    [InlineData("*", true)]
    [InlineData("", false)]
    public void ShouldReturnTrueWhenAssemblyNameMatchesPatternOtherwiseFalse(string patternString, bool expectedResult)
    {
      //GIVEN
      var project = new DotNetStandardProjectBuilder
      {
        AssemblyName = AnyRoot.Root.Any.String()
      }.Build();
      var pattern = new Glob(patternString);
      
      //WHEN
      var result = project.HasProjectAssemblyNameMatching(pattern);

      //THEN
      result.Should().Be(expectedResult);
    }

    private class DotNetStandardProjectBuilder
    {
      public DotNetStandardProject Build()
      {
        return new DotNetStandardProject(AssemblyName, 
          ProjectId, 
          ReferencedProjectIds, 
          PackageReferences, 
          AssemblyReferences,
          Files,
          NamespacesDependenciesCache, 
          Support);
      }

      public IReadOnlyList<PackageReference> PackageReferences { private get; set; } = AnyRoot.Root.Any.ReadOnlyList<PackageReference>();
      public IReadOnlyList<AssemblyReference> AssemblyReferences { private get; set; } =
        AnyRoot.Root.Any.ReadOnlyList<AssemblyReference>();
      public ProjectId[] ReferencedProjectIds { private get; set; } = AnyRoot.Root.Any.Array<ProjectId>();
      public ProjectId ProjectId { private get; set; } = AnyRoot.Root.Any.ProjectId();
      public string AssemblyName { private get; set; } = AnyRoot.Root.Any.String();
      public INScanSupport Support { private get; set; } = AnyRoot.Root.Any.Support();
      public string RootNamespace { get; set; } = AnyRoot.Root.Any.String(); //bug value object
      public IReadOnlyList<ISourceCodeFile> Files { get; set; } = AnyRoot.Root.Any.ReadOnlyList<ISourceCodeFile>();
      public INamespacesDependenciesCache NamespacesDependenciesCache { get; set; } = AnyRoot.Root.Any.Instance<INamespacesDependenciesCache>();
    }
  }

  
}