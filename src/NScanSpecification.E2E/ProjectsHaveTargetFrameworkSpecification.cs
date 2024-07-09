using static NScanSpecification.Lib.AutomationLayer.HasTargetFrameworkReportedMessage;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;

namespace NScanSpecification.E2E;

public class ProjectsHaveTargetFrameworkSpecification(ITestOutputHelper output)
{
  [Fact]
  public async Task ShouldReportSuccessWhenAllProjectsHaveSpecifiedFramework()
  {
    //GIVEN
    using var context = new NScanE2EDriver(output);
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
    using var context = new NScanE2EDriver(output);
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
