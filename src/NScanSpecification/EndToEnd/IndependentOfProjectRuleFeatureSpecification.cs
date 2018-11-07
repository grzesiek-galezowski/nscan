using System;
using System.Diagnostics.CodeAnalysis;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

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

        context.AddIndependentOfProjectRule(projectName, assemblyName);

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContainText(SuccessAssemblyRuleText(projectName, assemblyName));
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

        context.AddIndependentOfProjectRule(projectName, dependencyProjectName);

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContainText(DirectFailureAssemblyRuleText(projectName, dependencyProjectName));
        context.ShouldIndicateFailure();
      }
    }

    private string DirectFailureAssemblyRuleText(string projectName, string dependencyProjectName)
    {
      return $"[{projectName}] independentOf [project:{dependencyProjectName}]: [ERROR]{Environment.NewLine}" +
             $"Violation in path: [{projectName}]";
    }
    private static string SuccessAssemblyRuleText(string projectName, string dependencyProjectName)
    {
      return $"[{projectName}] independentOf [project:{dependencyProjectName}]: [OK]";
    }

  }
}