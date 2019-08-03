using TddXt.NScan.Specification.EndToEnd.AutomationLayer;
using Xunit;
using static TddXt.NScan.Specification.AutomationLayer.HasTargetFrameworkReportedMessage;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.EndToEnd
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
  }
}
