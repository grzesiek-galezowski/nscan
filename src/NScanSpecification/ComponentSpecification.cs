﻿using Xunit;
using static System.Environment;

namespace TddXt.NScan.Specification
{
  public class ComponentSpecification
  {
    //bug report should have correct return code!!!
    [Fact]
    public void ShouldReportAllSatisfiedRules()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("A");
      context.HasProject("B");

      context.AddIndependentOfRule("A", "B");

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText("[A] independentOf [B]: [OK]");
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

      context.AddIndependentOfRule("A", "B");

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

      context.AddIndependentOfRule("A", "C");

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

      context.AddIndependentOfRule("A", "D");
      context.AddIndependentOfRule("A", "B");

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

      context.AddIndependentOfRule("*.Domain", "*.Ports");

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[*.Domain] independentOf [*.Ports]: [ERROR]{NewLine}" +
                                      $"Violation in path: [Posts.Domain]->[Posts.Ports]");
      context.ShouldIndicateFailure();

    }

    //rule for bad projects
  }
}