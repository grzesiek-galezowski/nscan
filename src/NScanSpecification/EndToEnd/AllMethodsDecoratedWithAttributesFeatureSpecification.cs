using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.EndToEnd.AutomationLayer;
using Xunit;
using static TddXt.NScan.Specification.AutomationLayer.ReportedMessage;
using static TddXt.NScan.Specification.AutomationLayer.XmlSourceCodeFileBuilder;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;
using static TddXt.NScan.Specification.EndToEnd.AutomationLayer.ClassDeclarationBuilder;
using static TddXt.NScan.Specification.EndToEnd.AutomationLayer.MethodDeclarationBuilder;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class AllMethodsDecoratedWithAttributesFeatureSpecification
  {
    [Fact]
    public void ShouldRaiseErrorWhenMethodsMatchingPatternAreNotDecoratedWithAttributes()
    {
      //GIVEN
      using var context = new NScanE2EDriver();
      //bug none of this is implemented yet
      context.HasProject("MyProject")
        .With(File("lol.cs").With(
          Class("Class1").With(
            Method("ShouldA"),
            Method("ShouldB"))));

      context.Add(RuleRequiring().Project("*MyProject*").ToHaveAnnotatedMethods("*Specification", "Should*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasAttributesOnMessage.HasMethodsNotDecoratedWithAttribute("MyProject", "*Specification", "Should*")
          .Error()
          .NonCompliantMethodFound("Class1", "ShouldA")
          .NonCompliantMethodFound("Class1", "ShouldB"));
    }
  }
}
