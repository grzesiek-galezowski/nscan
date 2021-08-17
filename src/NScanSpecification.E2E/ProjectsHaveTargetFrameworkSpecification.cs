using System.Threading.Tasks;
using NScanSpecification.E2E.AutomationLayer;
using Xunit;
using Xunit.Abstractions;
using static NScanSpecification.Lib.AutomationLayer.HasTargetFrameworkReportedMessage;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;

namespace NScanSpecification.E2E
{
  public class ProjectsHaveTargetFrameworkSpecification
  {
    private readonly ITestOutputHelper _output;

    public ProjectsHaveTargetFrameworkSpecification(ITestOutputHelper output)
    {
      _output = output;
    }

    [Fact]
    public async Task ShouldReportSuccessWhenAllProjectsHaveSpecifiedFramework()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject")
        .WithTargetFramework(TargetFramework.RecentDotNet);
          
      context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework(TargetFramework.RecentDotNet));

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasFramework("*MyProject*", TargetFramework.RecentDotNet).Ok());
    }

    [Fact]
    public async Task ShouldReportErrorForProjectsThatDoNotHaveSpecifiedFramework()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject").WithTargetFramework(TargetFramework.RecentDotNet);

      context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.1"));

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasFramework("*MyProject*", "netstandard2.1").Error()
          .ProjectHasAnotherTargetFramework("MyProject", TargetFramework.RecentDotNet));
    }
  }
}
