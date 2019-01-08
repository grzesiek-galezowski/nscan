using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using TddXt.NScan.Specification.EndToEnd;
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
      context.ReportShouldContain(ReportedMessage.HasNoCircularUsings("*MyProject*").Ok());

    }
  }
}