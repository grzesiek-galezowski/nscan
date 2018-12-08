using Xunit;
using static System.Environment;
using static TddXt.NScan.Specification.Component.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
{
  public class CorrectNamespacesRuleFeatureSpecification
  {
    //TODO happy path

    [Fact]
    public void ShouldReportErrorWhenFileIsInWrongNamespace()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .WithFile("lol.cs", "WrongNamespace");
      context.Add(Rule().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText($"MyProject hasCorrectNamespace: [ERROR]{NewLine}" +
                                      $"MyProject has root namespace MyProject but the file lol.cs located in project root folder has incorrect namespace WrongNamespace");

      
    }

    //backlog nested namespaces
    //backlog multiple namespaces per file
  }
}
