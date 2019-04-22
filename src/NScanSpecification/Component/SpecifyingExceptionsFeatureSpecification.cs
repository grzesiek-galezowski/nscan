using System;
using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
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

      context.Add(DependencyRuleBuilder.RuleRequiring()
        .Project("*")
        .Except("*Specification*")
        .IndependentOfProject("*CompositionRoot*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ProjectIndependentOfMessage.ProjectIndependentOfProject(
          "* except *Specification*", "*CompositionRoot*"
          ).Ok());
      context.ShouldIndicateSuccess();

    }

    [Fact]
    public void ShouldReportFailureWhenPatternExclusionDoesNotRuleOutDependingThatMatchAlone()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("CompositionRoot");
      context.HasProject("CompositionRootSpecification").WithReferences("CompositionRoot");
      context.HasProject("CompositionRootTests").WithReferences("CompositionRoot");

      context.Add(DependencyRuleBuilder.RuleRequiring()
        .Project("*")
        .Except("*Tests*")
        .IndependentOfProject("*CompositionRoot*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ProjectIndependentOfMessage.ProjectIndependentOfProject(
          "* except *Tests*", "*CompositionRoot*").Error()
          .ViolationPath("CompositionRootSpecification", "CompositionRoot"));
      context.ReportShouldNotContainText("CompositionRootTests");
      context.ShouldIndicateFailure();

    }
  }
}