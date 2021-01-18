using NScanSpecification.Component.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.HasTargetFrameworkReportedMessage;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;

namespace NScanSpecification.Component
{
  public class HasTargetFrameworkRuleFeatureSpecification
  {
    public class ProjectsHaveTargetFrameworkSpecification
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
            .HasTargetFramework("netcoreapp2.1"));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(
          HasFramework("*MyProject*", "netcoreapp2.1").Ok());
      }

      [Fact]
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

      [Fact]
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

      [Fact]
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
}
