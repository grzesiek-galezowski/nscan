using TddXt.NScan.Specification.Component.AutomationLayer;
using Xunit;
using static TddXt.NScan.Specification.AutomationLayer.HasTargetFrameworkReportedMessage;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
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

      //TODO test on negative
      //TODO test on multiple negative
      //TODO test on filtering by pattern
    }
  }
}
