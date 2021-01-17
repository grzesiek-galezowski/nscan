using NScanSpecification.Component.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using Xunit;
using static NScanSpecification.Component.HasPropertyReportedMessage;
using static NScanSpecification.Lib.AutomationLayer.HasTargetFrameworkReportedMessage;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;

namespace NScanSpecification.Component
{
  public class HasPropertyRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenAllProjectsHaveSpecifiedFramework()
    {
      //GIVEN
      var context = new NScanDriver();

      context.HasProject("MyProject")
        .WithTargetFramework("netcoreapp2.1");

      context.Add(
        RuleDemandingThat().Project("*MyProject*")
          .HasProperty("TargetFramework", "netcoreapp2.1"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasProperty("*MyProject*", "TargetFramework", "netcoreapp2.1").Ok());
    }

    [Fact] //bug
    public void ShouldReportErrorForProjectThatDoesNotHaveSpecifiedFramework()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject").WithTargetFramework("netcoreapp3.1");

      context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.1"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasFramework("*MyProject*", "netstandard2.1").Error()
        .ProjectHasAnotherTargetFramework("MyProject", "netcoreapp3.1"));
    }

    [Fact] //bug
    public void ShouldReportErrorForMultipleProjectsThatDoNotHaveSpecifiedFramework()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject1").WithTargetFramework("netcoreapp3.1");
      context.HasProject("MyProject2").WithTargetFramework("netcoreapp3.1");

      context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.1"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasFramework("*MyProject*", "netstandard2.1").Error()
        .ProjectHasAnotherTargetFramework("MyProject1", "netcoreapp3.1")
        .ProjectHasAnotherTargetFramework("MyProject2", "netcoreapp3.1"));
    }

    [Fact] //bug
    public void ShouldNotReportErrorForProjectsThatDoNotMatchTheNamePattern()
    {
      //GIVEN
      var projectAssemblyNameThatDoesNotMatchRuleFilter = "Trolololo";
      var context = new NScanDriver();

      context.HasProject(projectAssemblyNameThatDoesNotMatchRuleFilter).WithTargetFramework("netcoreapp3.1");
      context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.1"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldNotContainText(projectAssemblyNameThatDoesNotMatchRuleFilter);
    }
  }
}
