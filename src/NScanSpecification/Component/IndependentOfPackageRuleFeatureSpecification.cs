using System.Diagnostics.CodeAnalysis;
using TddXt.AnyRoot.Strings;
using Xunit;
using static System.Environment;
using static TddXt.AnyRoot.Root;
using static TddXt.NScan.Specification.Component.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
{
  [SuppressMessage("ReSharper", "TestFileNameWarning")]
  public class IndependentOfPackageRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenNoProjectHasSpecifiedPackageReference()
    {
      //GIVEN
      var projectName = Any.String(); 
      var packageName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName);

      context.Add(Rule().Project(projectName).IndependentOfPackage(packageName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText(SuccessPackageRuleText(projectName, packageName));
      context.ShouldIndicateSuccess();
    }

    [Fact]
    public void ShouldReportFailureWhenProjectsHasSpecifiedPackageReferenceDirectly()
    {
      //GIVEN
      var projectName = Any.String();
      var packageName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName).WithPackages(packageName);

      context.Add(Rule().Project(projectName).IndependentOfPackage(packageName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText(DirectFailurePackageRuleText(projectName, packageName));
      context.ShouldIndicateFailure();
    }

    [Fact]
    public void ShouldReportIndirectRuleBreak()
    {
      //GIVEN
      var projectName = Any.String();
      var projectName2 = Any.String();
      var packageName = Any.String();
      var context = new NScanDriver();
      context.HasProject(projectName).WithReferences(projectName2);
      context.HasProject(projectName2).WithPackages(packageName);

      context.Add(Rule().Project(projectName).IndependentOfPackage(packageName));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText(
        IndirectFailurePackageRuleText(projectName, projectName2, packageName));
      context.ShouldIndicateFailure();
    }

    private string IndirectFailurePackageRuleText(string projectName, string projectName2, string packageName)
    {
      return $"[{projectName}] independentOf [package:{packageName}]: [ERROR]{NewLine}" +
             $"PathViolation in path: [{projectName}]->[{projectName2}]";
    }

    private string DirectFailurePackageRuleText(string projectName, string packageName)
    {
      return $"[{projectName}] independentOf [package:{packageName}]: [ERROR]{NewLine}" +
             $"PathViolation in path: [{projectName}]";
    }

    private static string SuccessPackageRuleText(string projectName, string packageName)
    {
      return $"[{projectName}] independentOf [package:{packageName}]: [OK]";
    }


  }
}