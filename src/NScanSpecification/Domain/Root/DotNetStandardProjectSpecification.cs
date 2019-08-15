using System.Collections.Generic;
using FluentAssertions;
using GlobExpressions;
using NScan.Domain.Domain.DependencyPathBasedRules;
using NScan.Domain.Domain.NamespaceBasedRules;
using NScan.Domain.Domain.ProjectScopedRules;
using NScan.Domain.Domain.Root;
using NScan.Lib;
using NScan.SharedKernel.SharedKernel;
using NScanSpecification.Lib;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class DotNetStandardProjectSpecification
  {
    [Fact]
    public void ShouldTellSolutionToResolveAllItsReferencesByIds()
    {
      //GIVEN
      var referencedProjects = Substitute.For<IReferencedProjects>();
      var project = new DotNetStandardProjectBuilder
      {
        ReferencedProjects = referencedProjects
      }.Build();
      var solution = Substitute.For<ISolutionContext>();

      //WHEN
      project.ResolveReferencesFrom(solution);

      //THEN
      referencedProjects.Received(1).ResolveFrom(project, solution);
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
    public void ShouldSayItIsARootWhenItHasNoReferencingProjectsOtherwiseNo()
    {
      //GIVEN
      var referencingProjects = Substitute.For<IReferencingProjects>();
      var areThereAnyReferencingProjects = Any.Boolean();
      var project = new DotNetStandardProjectBuilder
      {
        ReferencingProjects = referencingProjects
      }.Build();

      referencingProjects.AreEmpty().Returns(areThereAnyReferencingProjects);

      //WHEN
      var isRoot = project.IsRoot();

      //THEN
      isRoot.Should().Be(areThereAnyReferencingProjects);
    }

    [Fact]
    public void ShouldAddReferencingProjectToReferencingProjects()
    {
      //GIVEN
      var referencingProjects = Substitute.For<IReferencingProjects>();
      var projectId = Any.ProjectId();
      var referencingProject = Any.Instance<IDependencyPathBasedRuleTarget>();
      var project = new DotNetStandardProjectBuilder()
      {
        ReferencingProjects = referencingProjects
      }.Build();

      //WHEN
      project.AddReferencingProject(projectId, referencingProject);

      //THEN
      referencingProjects.Received(1).Put(projectId, referencingProject);
    }

    [Fact]
    public void ShouldDelegateFillingAllBranchesOfDependencyPathsToReferencedProjects()
    {
      //GIVEN
      var referencedProjects = Substitute.For<IReferencedProjects>();
      var project = new DotNetStandardProjectBuilder()
      {
        ReferencedProjects = referencedProjects
      }.Build();
      var dependencyPathInProgress = Substitute.For<IDependencyPathInProgress>();

      //WHEN
      project.FillAllBranchesOf(dependencyPathInProgress);

      //THEN
      referencedProjects.Received(1).FillAllBranchesOf(dependencyPathInProgress, project);
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
      var namespacesCache = Any.Instance<INamespacesDependenciesCache>();
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
      var namespacesCache = Any.Instance<INamespacesDependenciesCache>();
      var rule = Substitute.For<INamespacesBasedRule>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      var projectAssemblyName = Any.String();
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
      var project = new DotNetStandardProjectBuilder()
      {
        Files = files
      }.Build();
      var rule = Substitute.For<IProjectFilesetScopedRule>();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
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
        AssemblyName = Any.String()
      }.Build();
      var pattern = new Glob(patternString);
      
      //WHEN
      var result = project.HasProjectAssemblyNameMatching(pattern);

      //THEN
      result.Should().Be(expectedResult);
    }

    [Fact]
    public void ShouldApplyTargetFrameworkCheckToItsTargetFramework()
    {
      //GIVEN
      var targetFramework = Any.String();
      var targetFrameworkCheck = Substitute.For<ITargetFrameworkCheck>();
      var assemblyName = Any.String();
      var project = new DotNetStandardProjectBuilder
      {
        TargetFramework = targetFramework,
        AssemblyName = assemblyName
      }.Build();
      var report = Any.Instance<IAnalysisReportInProgress>();

      //WHEN
      project.ValidateTargetFrameworkWith(targetFrameworkCheck, report);

      //THEN
      targetFrameworkCheck.Received(1).ApplyTo(assemblyName, targetFramework, report);
    }


    private class DotNetStandardProjectBuilder
    {
      public DotNetStandardProject Build()
      {
        return new DotNetStandardProject(
          AssemblyName,
          ProjectId,
          TargetFramework,
          PackageReferences,
          AssemblyReferences,
          Files,
          NamespacesDependenciesCache,
          ReferencedProjects, 
          ReferencingProjects);
      }

      public IReferencedProjects ReferencedProjects { get; set; } = Any.Instance<IReferencedProjects>();

      public IReadOnlyList<PackageReference> PackageReferences { private get; set; } = Any.ReadOnlyList<PackageReference>();
      public IReadOnlyList<AssemblyReference> AssemblyReferences { private get; set; } =
        Any.ReadOnlyList<AssemblyReference>();

      public ProjectId ProjectId { private get; set; } = Any.ProjectId();
      public string AssemblyName { private get; set; } = Any.String();
      public IReadOnlyList<ISourceCodeFile> Files { private get; set; } = Any.ReadOnlyList<ISourceCodeFile>();
      public INamespacesDependenciesCache NamespacesDependenciesCache { private get; set; } = Any.Instance<INamespacesDependenciesCache>();
      public IReferencingProjects ReferencingProjects { private get; set; } = Any.Instance<IReferencingProjects>();
      public string TargetFramework { get; set; } = Any.String();
    }
  }

  
}