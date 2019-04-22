using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using Xunit;
using static TddXt.NScan.Specification.AutomationLayer.XmlSourceCodeFileBuilder;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
{
  public class NoCircularUsingsRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenThereAreNoCircularDependenciesBetweenNamespaces()
    {
      //GIVEN
      var context = new NScanDriver();

      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("lol1.cs", "MyProject"));
      context.Add(RuleRequiring().Project("*MyProject*").HasNoCircularUsings());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasNoCircularUsingsMessage.HasNoCircularUsings("*MyProject*").Ok());

    }
    
    [Fact]
    public void ShouldReportSuccessWhenAFileHasUsingForItsOwnNamespace()
    {
      //GIVEN
      var context = new NScanDriver();

      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("lol1.cs", "MyProject").Using("MyProject"));
      context.Add(RuleRequiring().Project("*MyProject*").HasNoCircularUsings());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasNoCircularUsingsMessage.HasNoCircularUsings("*MyProject*").Ok());

    }

    [Fact]
    public void ShouldReportErrorWhenACycleIsDiscovered()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("lol1.cs", "MyProject").Using("MyProject.Util"))
        .With(FileWithNamespace("lol2.cs", "MyProject.Util").Using("MyProject"));
      context.Add(RuleRequiring().Project("*MyProject*").HasNoCircularUsings());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasNoCircularUsingsMessage.HasNoCircularUsings("*MyProject*").Error()
        .CycleFound("MyProject", "MyProject", "MyProject.Util", "MyProject"));
    }

  }
}