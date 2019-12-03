using NScanSpecification.E2E.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.HasNoCircularUsingsMessage;
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
      using (var context = new NScanE2EDriver())
      {
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

    /*
    [Fact]
    public void ShouldReportErrorWhenACycleIsDiscovered()
    {
      //GIVEN
      using (var context = new NScanE2EDriver())
      {
        context.HasProject("MyProject")
          .WithRootNamespace("MyProject")
          .With(FileWithNamespace("lol1.cs", "MyProject").Using("MyProject.Util"))
          .With(FileWithNamespace("lol2.cs", "MyProject.Util").Using("MyProject"));
        context.Add(RuleDemandingThat().Project("*MyProject*").HasNoCircularUsings());

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(HasNoCircularUsings("*MyProject*").Error()
          .CycleFound("MyProject", "MyProject", "MyProject.Util", "MyProject"));
      }
    }
    */

  }
}