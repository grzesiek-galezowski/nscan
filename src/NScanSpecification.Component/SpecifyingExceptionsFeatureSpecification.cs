using NScanSpecification.Component.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using Xunit;

namespace NScanSpecification.Component;

public class SpecifyingExceptionsFeatureSpecification
{
  [Fact]
  public void ShouldReportSuccessWhenPatternExclusionRulesOutDependingThatWouldOtherwiseMatch()
  {
    //GIVEN
    var context = new NScanDriver();
    context.HasProject("CompositionRoot");
    context.HasProject("CompositionRootSpecification").WithReferences("CompositionRoot");

    context.Add(RuleBuilder.RuleDemandingThat()
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

    context.Add(RuleBuilder.RuleDemandingThat()
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