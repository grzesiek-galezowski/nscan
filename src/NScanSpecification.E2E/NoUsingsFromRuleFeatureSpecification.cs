using System.Threading.Tasks;
using NScanSpecification.E2E.AutomationLayer;
using Xunit;
using Xunit.Abstractions;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;
using static NScanSpecification.Lib.AutomationLayer.HasNoUsingsMessage;
using static NScanSpecification.Lib.AutomationLayer.SourceCodeFileDtoBuilder;

namespace NScanSpecification.E2E
{
  public class NoUsingsFromRuleFeatureSpecification
  {
    private readonly ITestOutputHelper _output;

    public NoUsingsFromRuleFeatureSpecification(ITestOutputHelper output)
    {
      _output = output;
    }

    [Fact]
    public async Task ShouldReportSuccessWhenThereAreNoCircularDependenciesBetweenNamespaces()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("Port.cs", "MyProject.Ports"))
        .With(FileWithNamespace("Adapter.cs", "MyProject.Adapters"));
      context.Add(RuleDemandingThat().Project("*MyProject*")
        .HasNoUsings(@from: "MyProject.Ports*", to: "MyProject.Adapters*"));

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasNoUsings("*MyProject*", "MyProject.Ports*", "MyProject.Adapters*").Ok());
    }

    [Fact]
    public async Task ShouldReportFailureWhenThereAreForbiddenUsingDependenciesBetweenNamespaces()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("Port.cs", "MyProject.Ports").Using("MyProject.Adapters"))
        .With(FileWithNamespace("Adapter.cs", "MyProject.Adapters"));
      context.Add(RuleDemandingThat().Project("*MyProject*")
        .HasNoUsings(@from: "MyProject.Ports*", to: "MyProject.Adapters*"));

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasNoUsings("*MyProject*", "MyProject.Ports*", "MyProject.Adapters*").Error()
        .UsingsPathFound("MyProject", "MyProject.Ports", "MyProject.Adapters"));
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