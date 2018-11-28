﻿using System.Diagnostics.CodeAnalysis;
using Xunit;
using static System.Environment;
using static TddXt.NScan.Specification.Component.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
{
  [SuppressMessage("ReSharper", "TestFileNameWarning")]
  public class IndependentOfProjectRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportAllSatisfiedRules()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("A");
      context.HasProject("B");

      context.Add(Rule().Project("A")
        .IndependentOfProject("B"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText("[A] independentOf [project:B]: [OK]");
      context.ShouldIndicateSuccess();
    }

    [Fact]
    public void ShouldDetectDirectRuleBreak()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("A").WithReferences("B", "C", "D");
      context.HasProject("B");
      context.HasProject("C");
      context.HasProject("D");

      context.Add(Rule().Project("A")
        .IndependentOfProject("B"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[A] independentOf [project:B]: [ERROR]{NewLine}" +
                                      "PathViolation in path: [A]->[B]");
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

      context.Add(Rule().Project("A").IndependentOfProject("C"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[A] independentOf [project:C]: [ERROR]{NewLine}" +
                                      "PathViolation in path: [A]->[B]->[C]");
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

      context.Add(Rule().Project("A").IndependentOfProject("D"));
      context.Add(Rule().Project("A").IndependentOfProject("B"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[A] independentOf [project:D]: [ERROR]{NewLine}" +
                                      $"PathViolation in path: [A]->[B]->[D]{NewLine}" +
                                      "PathViolation in path: [A]->[C]->[D]");
      context.ReportShouldContainText($"[A] independentOf [project:B]: [ERROR]{NewLine}" +
                                      "PathViolation in path: [A]->[B]");
      context.ShouldIndicateFailure();
    }

    [Fact]
    public void ShouldDetectRuleBreakWithGlobExpression()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("Posts.Domain").WithReferences("Posts.Ports");
      context.HasProject("Posts.Ports");

      context.Add(Rule().Project("*.Domain").IndependentOfProject("*.Ports"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[*.Domain] independentOf [project:*.Ports]: [ERROR]{NewLine}" +
                                      "PathViolation in path: [Posts.Domain]->[Posts.Ports]");
      context.ShouldIndicateFailure();

    }

  }
}