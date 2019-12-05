using NScanSpecification.Component.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;
using static NScanSpecification.Lib.AutomationLayer.SourceCodeFileDtoBuilder;

namespace NScanSpecification.E2E
{
  public class NoUsingsFromRuleFeatureSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenThereAreNoCircularDependenciesBetweenNamespaces()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("Port.cs", "MyProject.Ports"))
        .With(FileWithNamespace("Adapter.cs", "MyProject.Adapters"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasNoUsings(@from: "MyProject.Ports*", to: "MyProject.Adapters*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasNoUsingsMessage.HasNoUsings("*MyProject*", "MyProject.Ports*", "MyProject.Adapters*").Ok());
    }

  }
}