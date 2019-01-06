using TddXt.NScan.Specification.AutomationLayer;
using Xunit;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;
using static TddXt.NScan.Specification.AutomationLayer.XmlSourceCodeFileBuilder;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class NoCircularNamespaceDependenciesRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenThereAreNoCircularDependenciesBetweenNamespaces()
    {
      //GIVEN
      using (var context = new NScanE2EDriver())
      {
        context.HasProject("MyProject")
          .WithRootNamespace("MyProject")
          .With(FileWithNamespace("lol1.cs", "MyProject"));
        context.Add(Rule().Project("*MyProject*").HasNoCircularUsings());

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(ReportedMessage.HasNoCircularUsings("*MyProject*").Ok());
      }
    }

  }
}