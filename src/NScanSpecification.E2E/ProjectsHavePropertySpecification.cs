using System.Threading.Tasks;
using NScanSpecification.E2E.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using Xunit;
using Xunit.Abstractions;

namespace NScanSpecification.E2E
{
  public class ProjectsHavePropertySpecification
  {
    private readonly ITestOutputHelper _output;

    public ProjectsHavePropertySpecification(ITestOutputHelper output)
    {
      _output = output;
    }

    [Fact]
    public async Task ShouldReportSuccessWhenAllProjectsHaveSpecifiedProperty()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject")
        .WithTargetFramework(TargetFramework.RecentDotNet);
          
      context.Add(RuleBuilder.RuleDemandingThat()
        .Project("*MyProject*")
        .HasProperty("TargetFramework", TargetFramework.RecentDotNet));

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasPropertyReportedMessage
          .HasProperty("*MyProject*", "TargetFramework", TargetFramework.RecentDotNet).Ok());
    }

    [Fact]
    public async Task ShouldReportErrorForProjectsThatDoNotHaveSpecifiedFramework()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject").WithTargetFramework(TargetFramework.RecentDotNet);

      context.Add(RuleBuilder.RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.1"));

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasTargetFrameworkReportedMessage.HasFramework("*MyProject*", "netstandard2.1").Error()
          .ProjectHasAnotherTargetFramework("MyProject", TargetFramework.RecentDotNet));
    }
  }
}
