using System.Diagnostics.CodeAnalysis;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using Xunit;
using static TddXt.AnyRoot.Root;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
{
  [SuppressMessage("ReSharper", "TestFileNameWarning")]
  public class IndependentOfAssemblyRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenNoProjectHasSpecifiedAssemblyReference()
    {
      //GIVEN
      var projectName = Any.String(); 
      var assemblyName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName);

      context.Add(RuleRequiring().Project(projectName).IndependentOfAssembly(assemblyName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ProjectIndependentOfMessage.ProjectIndependentOfAssembly(projectName, assemblyName).Ok());
      context.ShouldIndicateSuccess();
    }

    [Fact]
    public void ShouldReportFailureWhenProjectsHasSpecifiedAssemblyReferenceDirectly()
    {
      //GIVEN
      var projectName = Any.String();
      var assemblyName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName).WithAssemblyReferences(assemblyName);

      context.Add(RuleRequiring().Project(projectName).IndependentOfAssembly(assemblyName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ProjectIndependentOfMessage.ProjectIndependentOfAssembly(projectName, assemblyName).Error()
          .ViolationPath(projectName));
      context.ShouldIndicateFailure();
    }

    [Fact]
    public void ShouldReportIndirectRuleBreak()
    {
      //GIVEN
      var projectName = Any.String("project1");
      var projectName2 = Any.String("project2");
      var assemblyName = Any.String("assembly");
      var context = new NScanDriver();
      context.HasProject(projectName).WithReferences(projectName2);
      context.HasProject(projectName2).WithAssemblyReferences(assemblyName);

      context.Add(RuleRequiring().Project(projectName).IndependentOfAssembly(assemblyName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ProjectIndependentOfMessage.ProjectIndependentOfAssembly(projectName, assemblyName).Error()
          .ViolationPath(projectName, projectName2));
      context.ShouldIndicateFailure();
    }
  }
}