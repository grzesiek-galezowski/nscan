using NScanSpecification.Component.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.HasTargetFrameworkReportedMessage;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;

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

        context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netcoreapp2.1"));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(HasFramework("*MyProject*", "netcoreapp2.1").Ok());
      }

      [Fact]
      public void ShouldReportErrorForProjectThatDoesNotHaveSpecifiedFramework()
      {
        var context = new NScanDriver();
        //GIVEN
        context.HasProject("MyProject").WithTargetFramework("netcoreapp2.2");

        context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.0"));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(HasFramework("*MyProject*", "netstandard2.0").Error()
          .ProjectHasAnotherTargetFramework("MyProject", "netcoreapp2.2"));
      }

      [Fact]
      public void ShouldReportErrorForMultipleProjectsThatDoNotHaveSpecifiedFramework()
      {
        var context = new NScanDriver();
        //GIVEN
        context.HasProject("MyProject1").WithTargetFramework("netcoreapp2.2");
        context.HasProject("MyProject2").WithTargetFramework("netcoreapp2.2");

        context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.0"));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(HasFramework("*MyProject*", "netstandard2.0").Error()
          .ProjectHasAnotherTargetFramework("MyProject1", "netcoreapp2.2")
          .ProjectHasAnotherTargetFramework("MyProject2", "netcoreapp2.2"));
      }

      [Fact]
      public void ShouldNotReportErrorForProjectsThatDoNotMatchTheNamePattern()
      {
        var projectAssemblyNameThatDoesNotMatchRuleFilter = "Trolololo";
        var context = new NScanDriver();
        //GIVEN
        context.HasProject(projectAssemblyNameThatDoesNotMatchRuleFilter).WithTargetFramework("netcoreapp2.2");

        context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.0"));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldNotContainText(projectAssemblyNameThatDoesNotMatchRuleFilter);
      }
    }
  }
}
