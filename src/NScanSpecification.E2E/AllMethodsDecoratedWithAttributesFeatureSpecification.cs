using NScanSpecification.Lib.AutomationLayer;
using TddXt.NScan.Specification.EndToEnd.AutomationLayer;
using Xunit;
using static TddXt.AnyRoot.Root;
using static NScanSpecification.Lib.AutomationLayer.XmlSourceCodeFileBuilder;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;
using static NScanSpecification.Lib.AutomationLayer.ClassDeclarationBuilder;
using static NScanSpecification.Lib.AutomationLayer.MethodDeclarationBuilder;

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
        .With(File(Any.CSharpFileName()).Containing(
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
