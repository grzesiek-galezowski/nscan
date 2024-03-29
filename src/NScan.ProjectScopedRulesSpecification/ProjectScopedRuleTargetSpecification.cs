﻿using NScan.Lib;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScanSpecification.Lib;

namespace NScan.ProjectScopedRulesSpecification;

public class ProjectScopedRuleTargetSpecification
{
  [Fact]
  public void ShouldApplyPropertyCheckToItsTargetFramework()
  {
    //GIVEN
    var propertyCheck = Substitute.For<IPropertyCheck>();
    var assemblyName = Any.Instance<AssemblyName>();
    var properties = Any.Dictionary<string, string>().ToHashMap();
    var project = new ProjectScopedRuleTarget(assemblyName, Any.Seq<ISourceCodeFileInNamespace>(), properties);
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
    var files = Any.Seq<ISourceCodeFileInNamespace>();
    var project =
      new ProjectScopedRuleTarget(
        Any.Instance<AssemblyName>(),
        files,
        Any.HashMap<string, string>());
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
    var assemblyName = Any.Instance<AssemblyName>();
    var project = new ProjectScopedRuleTarget(
      assemblyName,
      Any.Seq<ISourceCodeFileInNamespace>(),
      Any.HashMap<string, string>());

    //WHEN
    var hasProject = project.HasProjectAssemblyNameMatching(
      Pattern.WithoutExclusion(assemblyName.Value));

    //THEN
    hasProject.Should().BeTrue();
  }

  [Fact]
  public void ShouldSayItHasProjectAssemblyNameMatchingBlobPattern()
  {
    //GIVEN
    var assemblySuffix = Any.String();
    var project = new ProjectScopedRuleTarget(
      new AssemblyName($"{Any.String()}.{assemblySuffix}"), 
      Any.Seq<ISourceCodeFileInNamespace>(), 
      Any.HashMap<string, string>());
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
    var searchedAssemblyName = Any.Instance<AssemblyName>();
    var project = new ProjectScopedRuleTarget(
      Any.OtherThan(searchedAssemblyName),
      Any.Seq<ISourceCodeFileInNamespace>(),
      Any.HashMap<string, string>());

    //WHEN
    var hasProject = project.HasProjectAssemblyNameMatching(
      Pattern.WithoutExclusion(searchedAssemblyName.Value));

    //THEN
    hasProject.Should().BeFalse();
  }

  [Fact]
  public void ShouldReportItsAssemblyNameToReportWhenAskedToReportThatItIsMatched()
  {
    //GIVEN
    var report = Substitute.For<IAnalysisReportInProgress>();
    var assemblyName = Any.Instance<AssemblyName>();
    var project = new ProjectScopedRuleTarget(
      assemblyName, 
      Any.Seq<ISourceCodeFileInNamespace>(),
      Any.HashMap<string, string>());

    //WHEN
    project.AddInfoAboutMatchingPatternTo(report);

    //THEN
    report.Received(1).StartedCheckingTarget(assemblyName);
  }


}
