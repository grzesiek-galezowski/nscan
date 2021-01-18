using NScanSpecification.Component.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;
using static NScanSpecification.Lib.AutomationLayer.HasNoUsingsMessage;
using static NScanSpecification.Lib.AutomationLayer.SourceCodeFileDtoBuilder;

namespace NScanSpecification.Component
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
      context.ReportShouldContain(HasNoUsings("*MyProject*", "MyProject.Ports*", "MyProject.Adapters*").Ok());
    }

    [Fact]
    public void ShouldReportFailureWhenThereAreForbiddenUsingDependenciesBetweenNamespaces()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("Port.cs", "MyProject.Ports").Using("MyProject.Adapters"))
        .With(FileWithNamespace("Adapter.cs", "MyProject.Adapters"));
      context.Add(RuleDemandingThat().Project("*MyProject*")
        .HasNoUsings(@from: "MyProject.Ports*", to: "MyProject.Adapters*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasNoUsings("*MyProject*", "MyProject.Ports*", "MyProject.Adapters*").Error()
        .UsingsPathFound("MyProject", "MyProject.Ports", "MyProject.Adapters"));
    }

  }
}