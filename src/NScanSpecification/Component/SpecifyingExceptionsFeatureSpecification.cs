using System;
using Xunit;

namespace TddXt.NScan.Specification.Component
{
  public class SpecifyingExceptionsFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenPatternExclusionRulesOutDependingThatWouldOtherwiseMatch()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("CompositionRoot");
      context.HasProject("CompositionRootSpecification").WithReferences("CompositionRoot");

      context.Add(DependencyRuleBuilder.Rule()
        .Project("*")
        .Except("*Specification*")
        .IndependentOfProject("*CompositionRoot*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText("[* except *Specification*] independentOf [project:*CompositionRoot*]: [OK]");
      context.ShouldIndicateSuccess();

    }

    [Fact]
    public void ShouldReportFailureWhenPatternExclusionDoesNotRuleOutDependingThatMatchAlone()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("CompositionRoot");
      context.HasProject("CompositionRootSpecification").WithReferences("CompositionRoot");

      context.Add(DependencyRuleBuilder.Rule()
        .Project("*")
        .Except("*Tests*")
        .IndependentOfProject("*CompositionRoot*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"[* except *Tests*] independentOf [project:*CompositionRoot*]: [ERROR]{Environment.NewLine}" +
                                      "Violation in path: [CompositionRootSpecification]->[CompositionRoot]");
      context.ShouldIndicateFailure();

    }
  }
}