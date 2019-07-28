using TddXt.AnyExtensibility;
using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.EndToEnd.AutomationLayer;
using Xunit;
using static TddXt.AnyRoot.Root;
using static TddXt.NScan.Specification.AutomationLayer.ReportedMessage;
using static TddXt.NScan.Specification.AutomationLayer.XmlSourceCodeFileBuilder;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;
using static TddXt.NScan.Specification.AutomationLayer.ClassDeclarationBuilder;
using static TddXt.NScan.Specification.AutomationLayer.MethodDeclarationBuilder;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class AllMethodsDecoratedWithAttributesFeatureSpecification
  {
    [Fact]
    public void ShouldRaiseErrorWhenMethodsMatchingPatternAreNotDecoratedWithAttributes()
    {
      //GIVEN
      const string projectName = "MyProject";
      const string classInclusionPattern = "*Specification";
      const string methodInclusionPattern = "Should*";
      var projectAssemblyNameInclusionPattern = $"*{projectName}*";
      const string matchingMethod1Name = "ShouldA";
      const string matchingMethod2Name = "ShouldB";
      const string className = "MySpecification";

      using var context = new NScanE2EDriver();
      context.HasProject(projectName)
        .With(File(Any.CSharpFileName()).With(
          Class(className).With(
            Method(matchingMethod1Name),
            Method(matchingMethod2Name))));

      context.Add(RuleDemandingThat().Project(projectAssemblyNameInclusionPattern).ToHaveDecoratedMethods(classInclusionPattern, methodInclusionPattern));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasAttributesOnMessage.HasMethodsNotDecoratedWithAttribute(projectAssemblyNameInclusionPattern, classInclusionPattern, methodInclusionPattern)
          .Error()
          .NonCompliantMethodFound(className, matchingMethod1Name)
          .NonCompliantMethodFound(className, matchingMethod2Name));
    }
  }
}
