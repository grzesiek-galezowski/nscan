using System.Diagnostics.CodeAnalysis;
using NScanSpecification.E2E.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;

namespace NScanSpecification.E2E
{
  [SuppressMessage("ReSharper", "TestFileNameWarning")]
  public class IndependentOfProjectRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenNoProjectHasSpecifiedAssemblyReference()
    {
      //GIVEN
      var projectName = Any.String();
      var assemblyName = Any.String();
      using (var context = new NScanE2EDriver())
      {
        context.HasProject(projectName);

        context.Add(RuleDemandingThat().Project(projectName).IndependentOfProject(assemblyName));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(
          ProjectIndependentOfMessage.ProjectIndependentOfProject(projectName, assemblyName).Ok());
        context.ShouldIndicateSuccess();
      }
    }

    [Fact]
    public void ShouldReportFailureWhenProjectsHasSpecifiedAssemblyReferenceDirectly()
    {
      //GIVEN
      var projectName = Any.String();
      var dependencyProjectName = Any.String();
      using (var context = new NScanE2EDriver())
      {
        context.HasProject(dependencyProjectName);
        context.HasProject(projectName).WithAssemblyReferences(dependencyProjectName);

        context.Add(RuleDemandingThat().Project(projectName).IndependentOfProject(dependencyProjectName));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(
          ProjectIndependentOfMessage.ProjectIndependentOfProject(projectName, dependencyProjectName).Error()
            .ViolationPath(projectName));
        context.ShouldIndicateFailure();
      }
    }
  }
}