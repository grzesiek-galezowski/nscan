using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NScanSpecification.E2E.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using TddXt.AnyRoot.Strings;
using Xunit;
using Xunit.Abstractions;
using static TddXt.AnyRoot.Root;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;

namespace NScanSpecification.E2E
{
  [SuppressMessage("ReSharper", "TestFileNameWarning")]
  public class IndependentOfProjectRuleFeatureSpecification
  {
    private readonly ITestOutputHelper _output;

    public IndependentOfProjectRuleFeatureSpecification(ITestOutputHelper output)
    {
      _output = output;
    }

    [Fact]
    public async Task ShouldReportSuccessWhenNoProjectHasSpecifiedAssemblyReference()
    {
      //GIVEN
      var projectName = Any.String();
      var assemblyName = Any.String();
      using var context = new NScanE2EDriver(_output);
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
    public async Task ShouldReportFailureWhenProjectsHasSpecifiedAssemblyReferenceDirectly()
    {
      //GIVEN
      var projectName = Any.String();
      var dependencyProjectName = Any.String();
      using var context = new NScanE2EDriver(_output);
      context.HasProject(dependencyProjectName);
      context.HasProject(projectName).WithAssemblyReferences(dependencyProjectName);

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
}