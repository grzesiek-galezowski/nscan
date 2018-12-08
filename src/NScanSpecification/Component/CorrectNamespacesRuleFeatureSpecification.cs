using Xunit;
using static System.Environment;
using static TddXt.NScan.Specification.Component.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
{
  public class CorrectNamespacesRuleFeatureSpecification
  {
    //TODO happy path

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
      context.ReportShouldContainText($"*MyProject* hasCorrectNamespaces: [OK]");
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
      context.ReportShouldContainText($"*MyProject* hasCorrectNamespaces: [OK]");
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
      context.ReportShouldContainText($"*MyProject* hasCorrectNamespaces: [ERROR]{NewLine}" +
                                      $"MyProject has root namespace MyProject but the file lol1.cs located in project root folder has incorrect namespace WrongNamespace{NewLine}" +
                                      $"MyProject has root namespace MyProject but the file lol2.cs located in project root folder has incorrect namespace WrongNamespace");
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
      context.ReportShouldContainText($"*MyProject* hasCorrectNamespaces: [ERROR]{NewLine}" + 
                                      $"MyProject1 has root namespace MyProject1 but the file lol1.cs located in project root folder has incorrect namespace WrongNamespace{NewLine}" +
                                      $"MyProject1 has root namespace MyProject1 but the file lol2.cs located in project root folder has incorrect namespace WrongNamespace{NewLine}" +
                                      $"MyProject2 has root namespace MyProject2 but the file lol1.cs located in project root folder has incorrect namespace WrongNamespace{NewLine}" +
                                      "MyProject2 has root namespace MyProject2 but the file lol2.cs located in project root folder has incorrect namespace WrongNamespace");
      context.ReportShouldNotContainText("lol3");

    }

    //backlog nested namespaces
    //backlog multiple namespaces per file
    //backlog multiple namespaces
    //backlog non-root files
  }
}
