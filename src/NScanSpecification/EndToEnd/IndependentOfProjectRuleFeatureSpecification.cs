using System.Diagnostics.CodeAnalysis;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Specification.AutomationLayer;
using Xunit;
using static TddXt.AnyRoot.Root;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.EndToEnd
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

        context.Add(RuleRequiring().Project(projectName).IndependentOfProject(assemblyName));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(
          ReportedMessage.ProjectIndependentOfProject(projectName, assemblyName).Ok());
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

        context.Add(RuleRequiring().Project(projectName).IndependentOfProject(dependencyProjectName));

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(
          ReportedMessage.ProjectIndependentOfProject(projectName, dependencyProjectName).Error()
            .ViolationPath(projectName));
        context.ShouldIndicateFailure();
      }
    }
  }
}