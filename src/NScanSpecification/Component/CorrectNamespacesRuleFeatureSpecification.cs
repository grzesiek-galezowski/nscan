using Xunit;
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
        .WithFile("lol.cs", "namespace WrongNamespace {}");
      context.Add(Rule().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContainText("trolololo failed, MyProject->lol.cs " +
                                      "should be in namespace MyProject, but was in namespace WrongNamespace");

      
    }

    //backlog nested namespaces
    //backlog multiple namespaces per file
  }
}
