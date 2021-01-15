using System.Collections.Generic;
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
    public void ShouldApplyPropertyCheckToItsTargetFramework()
    {
      //GIVEN
      var propertyCheck = Substitute.For<IPropertyCheck>();
      var assemblyName = Any.String();
      var properties = Any.ReadOnlyDictionary<string, string>();
      var project = new ProjectScopedRuleTarget(
        assemblyName, 
        Any.ReadOnlyList<ISourceCodeFileInNamespace>(),
        properties);
      var report = Any.Instance<IAnalysisReportInProgress>();

      //WHEN
      project.ValidateProperty(propertyCheck, report);

      //THEN
      propertyCheck.Received(1).ApplyTo(assemblyName, properties, report);
    }

    [Fact]
    public void ShouldPassAllItsFilesToProjectScopedRuleAlongWithRootNamespace()
    {
      //GIVEN
      var files = Any.ReadOnlyList<ISourceCodeFileInNamespace>();
      var project = new ProjectScopedRuleTarget(
        Any.String(), 
        files,
        Any.ReadOnlyDictionary<string, string>());
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
        Any.ReadOnlyDictionary<string, string>());

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
        Any.ReadOnlyDictionary<string, string>());
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
        Any.ReadOnlyDictionary<string, string>());

      //WHEN
      var hasProject = project.HasProjectAssemblyNameMatching(Pattern.WithoutExclusion(searchedAssemblyName));

      //THEN
      hasProject.Should().BeFalse();
    }
  }
}
