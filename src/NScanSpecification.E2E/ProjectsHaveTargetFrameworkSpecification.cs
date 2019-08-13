using NScanSpecification.E2E.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.HasTargetFrameworkReportedMessage;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;

namespace NScanSpecification.E2E
{
  public class ProjectsHaveTargetFrameworkSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenAllProjectsHaveSpecifiedFramework()
    {
      //GIVEN
      using (var context = new NScanE2EDriver())
      {
        context.HasProject("MyProject")
          .WithTargetFramework("netcoreapp2.2");
          
        context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netcoreapp2.2"));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(HasFramework("*MyProject*", "netcoreapp2.2").Ok());
      }
    }

    [Fact]
    public void ShouldReportErrorForProjectsThatDoNotHaveSpecifiedFramework()
    {
      //GIVEN
      using (var context = new NScanE2EDriver())
      {
        context.HasProject("MyProject")
          .WithTargetFramework("netcoreapp2.2");

        context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.0"));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(HasFramework("*MyProject*", "netstandard2.0").Error()
          .ProjectHasAnotherTargetFramework("MyProject", "netcoreapp2.2"));
      }
    }
  }
}
