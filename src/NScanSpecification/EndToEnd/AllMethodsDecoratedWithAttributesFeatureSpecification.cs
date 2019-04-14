using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using TddXt.NScan.Specification.EndToEnd.AutomationLayer;
using Xunit;

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
        context.HasProject("MyProject")
          .WithRootNamespace("MyProject")
          .With(XmlSourceCodeFileBuilder.FileWithNamespace("lol1.cs", "MyProject").Using("MyProject.Util"))
          .With(XmlSourceCodeFileBuilder.FileWithNamespace("lol2.cs", "MyProject.Util").Using("MyProject"));
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
