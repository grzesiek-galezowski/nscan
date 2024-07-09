using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;

namespace NScanSpecification.E2E;

public record IndependentOfProjectRuleFeatureSpecification(ITestOutputHelper Output)
{
  [Fact]
  public async Task ShouldReportSuccessWhenNoProjectHasSpecifiedAssemblyReference()
  {
    //GIVEN
    var projectName = Any.String();
    var assemblyName = Any.String();
    using var context = new NScanE2EDriver(Output);
    context.HasProject(projectName);

    context.Add(RuleDemandingThat().Project(projectName).IndependentOfProject(assemblyName));

    //WHEN
    await context.PerformAnalysis();

    //THEN
    context.ReportShouldContain(
      ProjectIndependentOfMessage.ProjectIndependentOfProject(projectName, assemblyName).Ok());
    context.ShouldIndicateSuccess();
  }

  [Fact]
  public async Task ShouldReportFailureWhenProjectsHasSpecifiedProjectReferenceDirectly()
  {
    //GIVEN
    var projectName = Any.String();
    var dependencyProjectName = Any.String();
    using var context = new NScanE2EDriver(Output);
    context.HasProject(dependencyProjectName);
    context.HasProject(projectName).WithReferences(dependencyProjectName);

    context.Add(RuleDemandingThat().Project(projectName).IndependentOfProject(dependencyProjectName));

    //WHEN
    await context.PerformAnalysis();

    //THEN
    context.ReportShouldContain(
      ProjectIndependentOfMessage.ProjectIndependentOfProject(projectName, dependencyProjectName).Error()
        .ViolationPath(projectName));
    context.ShouldIndicateFailure();
  }
}
