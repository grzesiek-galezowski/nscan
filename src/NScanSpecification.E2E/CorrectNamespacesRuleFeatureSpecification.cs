using System.Threading.Tasks;
using NScanSpecification.E2E.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using Xunit;
using Xunit.Abstractions;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;
using static NScanSpecification.Lib.AutomationLayer.SourceCodeFileDtoBuilder;

namespace NScanSpecification.E2E
{
  public class CorrectNamespacesRuleFeatureSpecification
  {
    private readonly ITestOutputHelper _output;

    public CorrectNamespacesRuleFeatureSpecification(ITestOutputHelper output)
    {
      _output = output;
    }

    [Fact]
    public async Task ShouldNotReportErrorWhenMultipleFilesOfSingleProjectAreInCorrectNamespace()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("lol1.cs", "MyProject"))
        .With(FileWithNamespace("lol2.cs", "MyProject"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasCorrectNamespacesMessage.HasCorrectNamespaces("*MyProject*").Ok());
    }

    [Fact]
    public async Task
      ShouldReportErrorWhenMultipleFilesOfSingleProjectAreInWrongNamespaceEvenThoughSomeAreInTheRightOne()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("lol1.cs", "WrongNamespace"))
        .With(FileWithNamespace("lol2.cs", "WrongNamespace"))
        .With(FileWithNamespace("lol3.cs", "MyProject"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasCorrectNamespacesMessage.HasCorrectNamespaces("*MyProject*").Error()
          .ExpectedNamespace("MyProject", "MyProject").ButFoundIncorrectNamespaceFor("lol1.cs", "WrongNamespace")
          .ExpectedNamespace("MyProject", "MyProject").ButFoundIncorrectNamespaceFor("lol2.cs", "WrongNamespace"));
      context.ReportShouldNotContainText("lol3");
    }

    [Fact]
    public async Task
      ShouldReportErrorWhenMultipleNestedFilesOfSingleProjectAreInWrongNamespaceEvenThoughSomeAreInTheRightOne()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("Domain\\lol4.cs", "MyProject.Domain"))
        .With(FileWithNamespace("Domain\\lol5.cs", "MyProject"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasCorrectNamespacesMessage.HasCorrectNamespaces("*MyProject*").Error()
          .ExpectedNamespace("MyProject", "MyProject")
          .ButFoundIncorrectNamespaceFor("Domain\\lol5.cs", "MyProject"));
      context.ReportShouldNotContainText("lol4");
    }

    [Fact]
    public async Task ShouldIgnoreObjFolderWhenCheckingProjectScopedRule()
    {
      //GIVEN
      using var context = new NScanE2EDriver(_output);
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("obj\\lol4.cs", "Trolololo"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasCorrectNamespacesMessage.HasCorrectNamespaces("*MyProject*").Ok());
      context.ReportShouldNotContainText("lol4");
    }

  }
}
