using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace TddXt.NScan.Specification.Component
{
  [SuppressMessage("ReSharper", "TestFileNameWarning")]
  public class IndependentOfPackageRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportAllSatisfiedRules()
    {
      //GIVEN
      var projectName = "A";
      var packageName = "WhateverPackage";
      var context = new NScanDriver();
      context.HasProject(projectName);

      context.AddIndependentOfPackageRule(projectName, packageName);

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText(SuccessPackageRuleText(projectName, packageName));
      context.ShouldIndicateSuccess();
    }

    private static string SuccessPackageRuleText(string projectName, string packageName)
    {
      return $"[{projectName}] independentOf package:[{packageName}]: [OK]";
    }

    //bug
    /*
    [Fact]
    public void ShouldDetectDirectRuleBreak()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("A").WithReferences("B", "C", "D");
      context.HasProject("B");
      context.HasProject("C");
      context.HasProject("D");

      context.AddIndependentOfProjectRule("A", "B");

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[A] independentOf [B]: [ERROR]{NewLine}" +
                                      $"Violation in path: [A]->[B]");
      context.ShouldIndicateFailure();
    }

    [Fact]
    public void ShouldDetectIndirectRuleBreak()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("A").WithReferences("B");
      context.HasProject("B").WithReferences("C");
      context.HasProject("C");
      context.HasProject("D");

      context.AddIndependentOfProjectRule("A", "C");

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[A] independentOf [C]: [ERROR]{NewLine}" +
                                      "Violation in path: [A]->[B]->[C]");
      context.ShouldIndicateFailure();
    }

    [Fact]
    public void ShouldDetectMultipleIndirectRuleBreaksWithMultipleViolationPaths()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("A").WithReferences("B", "C");
      context.HasProject("B").WithReferences("D");
      context.HasProject("C").WithReferences("D");
      context.HasProject("D");

      context.AddIndependentOfProjectRule("A", "D");
      context.AddIndependentOfProjectRule("A", "B");

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[A] independentOf [D]: [ERROR]{NewLine}" +
                                      $"Violation in path: [A]->[B]->[D]{NewLine}" +
                                      $"Violation in path: [A]->[C]->[D]");
      context.ReportShouldContainText($"[A] independentOf [B]: [ERROR]{NewLine}" +
                                      "Violation in path: [A]->[B]");
      context.ShouldIndicateFailure();
    }

    [Fact]
    public void ShouldDetectRuleBreakWithRegularExpression()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("Posts.Domain").WithReferences("Posts.Ports");
      context.HasProject("Posts.Ports");

      context.AddIndependentOfProjectRule("*.Domain", "*.Ports");

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[*.Domain] independentOf [*.Ports]: [ERROR]{NewLine}" +
                                      $"Violation in path: [Posts.Domain]->[Posts.Ports]");
      context.ShouldIndicateFailure();

    }

    */
    //rule for bad projects
  }
}