using System.Threading;
using System.Threading.Tasks;
using NScanSpecification.E2E.AutomationLayer;
using NScanSpecification.Lib;
using NScanSpecification.Lib.AutomationLayer;
using Xunit;
using Xunit.Abstractions;
using static TddXt.AnyRoot.Root;
using static NScanSpecification.Lib.AutomationLayer.SourceCodeFileDtoBuilder;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;
using static NScanSpecification.Lib.AutomationLayer.ClassDeclarationBuilder;
using static NScanSpecification.Lib.AutomationLayer.MethodDeclarationBuilder;

namespace NScanSpecification.E2E
{
  public class AllMethodsDecoratedWithAttributesFeatureSpecification
  {
    private readonly ITestOutputHelper _output;

    public AllMethodsDecoratedWithAttributesFeatureSpecification(ITestOutputHelper output)
    {
      _output = output;
    }

    [Fact]
    public async Task ShouldRaiseErrorWhenMethodsMatchingPatternAreNotDecoratedWithAttributes()
    {
      //GIVEN
      const string projectName = "MyProject";
      const string classInclusionPattern = "*Specification";
      const string methodInclusionPattern = "Should*";
      var projectAssemblyNameInclusionPattern = $"*{projectName}*";
      const string matchingMethod1Name = "ShouldA";
      const string matchingMethod2Name = "ShouldB";
      const string className = "MySpecification";

      using var context = new NScanE2EDriver(_output);
      context.HasProject(projectName)
        .With(File(Any.CSharpFileName()).Containing(
          Class(className).With(
            Method(matchingMethod1Name),
            Method(matchingMethod2Name))));

      context.Add(RuleDemandingThat()
        .Project(projectAssemblyNameInclusionPattern)
        .HasDecoratedMethods(classInclusionPattern, methodInclusionPattern));

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasAttributesOnMessage.HasMethodsNotDecoratedWithAttribute(projectAssemblyNameInclusionPattern, classInclusionPattern, methodInclusionPattern)
          .Error()
          .NonCompliantMethodFound(className, matchingMethod1Name)
          .NonCompliantMethodFound(className, matchingMethod2Name));
    }
  }
}
