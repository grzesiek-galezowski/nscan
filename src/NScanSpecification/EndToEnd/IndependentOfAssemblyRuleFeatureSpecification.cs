using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.EndToEnd
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
      using (var context = new NScanE2EDriver())
      {
        context.HasProject(projectName);

        context.AddIndependentOfAssemblyRule(projectName, assemblyName);

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContainText(SuccessAssemblyRuleText(projectName, assemblyName));
        context.ShouldIndicateSuccess();
      }
    }

    /*
    [Fact]
    public void ShouldReportFailureWhenProjectsHasSpecifiedAssemblyReferenceDirectly()
    {
      //GIVEN
      var projectName = Any.String();
      var assemblyName = Any.String();
      using (var context = new NScanE2EDriver())
      {
        context.HasProject(projectName).WithAssemblyReferences(assemblyName);

        context.AddIndependentOfAssemblyRule(projectName, assemblyName);

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContainText(DirectFailureAssemblyRuleText(projectName, assemblyName));
        context.ShouldIndicateFailure();
      }
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

      context.AddIndependentOfAssemblyRule(projectName, assemblyName);

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText(
        IndirectFailureAssemblyRuleText(projectName, projectName2, assemblyName));
      context.ShouldIndicateFailure();
    }

    private string IndirectFailureAssemblyRuleText(string projectName, string projectName2, string packageName)
    {
      return $"[{projectName}] independentOf [assembly:{packageName}]: [ERROR]{Environment.NewLine}" +
             $"Violation in path: [{projectName}]->[{projectName2}]";
    }

    private string DirectFailureAssemblyRuleText(string projectName, string packageName)
    {
      return $"[{projectName}] independentOf [assembly:{packageName}]: [ERROR]{Environment.NewLine}" +
             $"Violation in path: [{projectName}]";
    }

    */
    private static string SuccessAssemblyRuleText(string projectName, string packageName)
    {
      return $"[{projectName}] independentOf [assembly:{packageName}]: [OK]";
    }

  }
}