using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TddXt.AnyRoot.Strings;
using Xunit;
using static System.Environment;
using static TddXt.AnyRoot.Root;
using static TddXt.NScan.Specification.Component.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
{
  [SuppressMessage("ReSharper", "TestFileNameWarning")]
  public class IndependentOfAssemblyRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenNoProjectHasSpecifiedAssemblyReference()
    {
      //GIVEN
      var projectName = Any.String(); 
      var assemblyName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName);

      context.Add(Rule().Project(projectName).IndependentOfAssembly(assemblyName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText(RuleMessage.SuccessAssemblyRuleText(projectName, assemblyName));
      context.ShouldIndicateSuccess();
    }

    [Fact]
    public void ShouldReportFailureWhenProjectsHasSpecifiedAssemblyReferenceDirectly()
    {
      //GIVEN
      var projectName = Any.String();
      var assemblyName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName).WithAssemblyReferences(assemblyName);

      context.Add(Rule().Project(projectName).IndependentOfAssembly(assemblyName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText(RuleMessage.DirectFailureAssemblyRuleText(projectName, assemblyName));
      context.ShouldIndicateFailure();
    }

    [Fact]
    public void ShouldReportIndirectRuleBreak()
    {
      //GIVEN
      var projectName = Any.String("project1");
      var projectName2 = Any.String("project2");
      var assemblyName = Any.String("assembly");
      var context = new NScanDriver();
      context.HasProject(projectName).WithReferences(projectName2);
      context.HasProject(projectName2).WithAssemblyReferences(assemblyName);

      context.Add(Rule().Project(projectName).IndependentOfAssembly(assemblyName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText(RuleMessage.IndirectFailureAssemblyRuleText(projectName, projectName2, assemblyName));
      context.ShouldIndicateFailure();
    }
  }
}