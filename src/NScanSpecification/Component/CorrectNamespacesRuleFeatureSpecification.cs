using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using Xunit;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
{
  public class CorrectNamespacesRuleFeatureSpecification
  {
    [Fact]
    public void ShouldNotReportErrorWhenMultipleFilesOfSingleProjectAreInCorrectNamespace()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .WithFile("lol1.cs", "MyProject")
        .WithFile("lol2.cs", "MyProject");
      context.Add(Rule().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(ReportedMessage.HasCorrectNamespaces("*MyProject*").Ok());
    }

    [Fact]
    public void ShouldNotReportErrorWhenThereAreNoFilesInMatchedProjects()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject1")
        .WithRootNamespace("MyProject2");
      context.HasProject("MyProject2")
        .WithRootNamespace("MyProject2");
      context.Add(Rule().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(ReportedMessage.HasCorrectNamespaces($"*MyProject*").Ok());
    }


    [Fact]
    public void ShouldReportErrorWhenMultipleFilesOfSingleProjectAreInWrongNamespaceEvenThoughSomeAreInTheRightOne()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .WithFile("lol1.cs", "WrongNamespace")
        .WithFile("lol2.cs", "WrongNamespace")
        .WithFile("lol3.cs", "MyProject");
      context.Add(Rule().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ReportedMessage.HasCorrectNamespaces("*MyProject*").Error()
          .ExpectedNamespace("MyProject", "MyProject")
          .ButFoundIncorrectNamespaceFor("lol1.cs", "WrongNamespace")
          .ExpectedNamespace("MyProject", "MyProject")
          .ButFoundIncorrectNamespaceFor("lol2.cs", "WrongNamespace"));
      context.ReportShouldNotContainText("lol3");
    }

    [Fact]
    public void ShouldReportErrorWhenMultipleProjectsHaveFilesInWrongNamespaces()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject1")
        .WithRootNamespace("MyProject1")
        .WithFile("lol1.cs", "WrongNamespace")
        .WithFile("lol2.cs", "WrongNamespace")
        .WithFile("lol3.cs", "MyProject1");
      context.HasProject("MyProject2")
        .WithRootNamespace("MyProject2")
        .WithFile("lol1.cs", "WrongNamespace")
        .WithFile("lol2.cs", "WrongNamespace")
        .WithFile("lol3.cs", "MyProject2");
      context.Add(Rule().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        ReportedMessage.HasCorrectNamespaces("*MyProject*").Error()
          .ExpectedNamespace("MyProject1", "MyProject1")
          .ButFoundIncorrectNamespaceFor("lol1.cs", "WrongNamespace")
          .ExpectedNamespace("MyProject1", "MyProject1")
          .ButFoundIncorrectNamespaceFor("lol2.cs", "WrongNamespace")
          .ExpectedNamespace("MyProject2", "MyProject2")
          .ButFoundIncorrectNamespaceFor("lol1.cs", "WrongNamespace")
          .ExpectedNamespace("MyProject2", "MyProject2")
          .ButFoundIncorrectNamespaceFor("lol2.cs", "WrongNamespace"));
      context.ReportShouldNotContainText("lol3");
    }

    [Fact]
    public void
      ShouldReportErrorWhenMultipleNestedFilesOfSingleProjectAreInWrongNamespaceEvenThoughSomeAreInTheRightOne()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .WithFile("Domain\\lol4.cs", "MyProject.Domain")
        .WithFile("Domain\\lol5.cs", "MyProject");
      context.Add(Rule().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(ReportedMessage
        .HasCorrectNamespaces("*MyProject*").Error()
        .ExpectedNamespace("MyProject", "MyProject")
        .ButFoundIncorrectNamespaceFor("Domain\\lol5.cs", "MyProject"));
      context.ReportShouldNotContainText("lol4");
    }

    //backlog nested namespaces
    //backlog multiple namespaces per file
    //backlog for all such unmade design decisions, throw exception with reference to github issue
    //        and allow disabling detection of such features via config or commandline parameters
  }
}
