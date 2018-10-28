using System;
using Xunit;

namespace TddXt.NScan.Specification.Component
{
  public class IndependentOfProjectRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportAllSatisfiedRules()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("A");
      context.HasProject("B");

      context.AddIndependentOfProjectRule("A", "B");

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

      context.AddIndependentOfProjectRule("A", "B");

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[A] independentOf [B]: [ERROR]{Environment.NewLine}" +
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
      context.ReportShouldContainText($"[A] independentOf [C]: [ERROR]{Environment.NewLine}" +
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
      context.ReportShouldContainText($"[A] independentOf [D]: [ERROR]{Environment.NewLine}" +
                                      $"Violation in path: [A]->[B]->[D]{Environment.NewLine}" +
                                      $"Violation in path: [A]->[C]->[D]");
      context.ReportShouldContainText($"[A] independentOf [B]: [ERROR]{Environment.NewLine}" +
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
      context.ReportShouldContainText($"[*.Domain] independentOf [*.Ports]: [ERROR]{Environment.NewLine}" +
                                      $"Violation in path: [Posts.Domain]->[Posts.Ports]");
      context.ShouldIndicateFailure();

    }

    //rule for bad projects
  }
}