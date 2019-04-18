using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using TddXt.NScan.Specification.EndToEnd.AutomationLayer;
using Xunit;
using static TddXt.NScan.Specification.AutomationLayer.XmlSourceCodeFileBuilder;
using static TddXt.NScan.Specification.EndToEnd.AutomationLayer.ClassDeclarationBuilder;
using static TddXt.NScan.Specification.EndToEnd.AutomationLayer.MethodDeclarationBuilder;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class AllMethodsDecoratedWithAttributesFeatureSpecification
  {
    [Fact]
    public void ShouldTODO() //bug
    {
      //GIVEN
      using (var context = new NScanE2EDriver())
      {
        //bug none of this is implemented yet
        context.HasProject("MyProject")
          .With(File("lol.cs").With(
            Class("Class1").With(
              Method("A").DecoratedWithAttribute("x"))));

        //bug

        context.Add(DependencyRuleBuilder.RuleRequiring().Project("*MyProject*").HasNoCircularUsings());

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(ReportedMessage.HasNoCircularUsings("*MyProject*").Error()
          .CycleFound("MyProject", "MyProject", "MyProject.Util", "MyProject"));
      }

    }
  }
}
