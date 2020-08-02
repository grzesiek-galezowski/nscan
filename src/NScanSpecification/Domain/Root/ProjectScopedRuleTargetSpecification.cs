using FluentAssertions;
using NScan.Lib;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class ProjectScopedRuleTargetSpecification
  {
    [Fact]
    public void ShouldApplyTargetFrameworkCheckToItsTargetFramework()
    {
      //GIVEN
      var targetFramework = Any.String();
      var targetFrameworkCheck = Substitute.For<ITargetFrameworkCheck>();
      var assemblyName = Any.String();
      var project = new ProjectScopedRuleTarget(
        assemblyName, 
        Any.ReadOnlyList<ISourceCodeFileInNamespace>(), 
        targetFramework);
      var report = Any.Instance<IAnalysisReportInProgress>();

      //WHEN
      project.ValidateTargetFrameworkWith(targetFrameworkCheck, report);

      //THEN
      targetFrameworkCheck.Received(1).ApplyTo(assemblyName, targetFramework, report);
    }

    [Fact]
    public void ShouldPassAllItsFilesToProjectScopedRuleAlongWithRootNamespace()
    {
      //GIVEN
      var files = Any.ReadOnlyList<ISourceCodeFileInNamespace>();
      var project = new ProjectScopedRuleTarget(Any.String(), files, Any.String());
      var rule = Substitute.For<IProjectFilesetScopedRule>();
      var report = Any.Instance<IAnalysisReportInProgress>();

      //WHEN
      project.AnalyzeFiles(rule, report);

      //THEN
      rule.Received(1).Check(files, report);
    }

    [Fact]
    public void ShouldSayItHasProjectIdMatchingTheOneItWasCreatedWith()
    {
      //GIVEN
      var assemblyName = Any.String();
      var project = new ProjectScopedRuleTarget(
        assemblyName, 
        Any.ReadOnlyList<ISourceCodeFileInNamespace>(), 
        Any.String());

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
      var project = new ProjectScopedRuleTarget(
        assemblyName, 
        Any.ReadOnlyList<ISourceCodeFileInNamespace>(), 
        Any.String());
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
      var project = new ProjectScopedRuleTarget(
        Any.OtherThan(searchedAssemblyName), 
        Any.ReadOnlyList<ISourceCodeFileInNamespace>(), 
        Any.String());

      //WHEN
      var hasProject = project.HasProjectAssemblyNameMatching(Pattern.WithoutExclusion(searchedAssemblyName));

      //THEN
      hasProject.Should().BeFalse();
    }
  }
}