using System.IO;
using NScanSpecification.Component.AutomationLayer;
using NScanSpecification.Lib.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.SourceCodeFileDtoBuilder;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;

namespace NScanSpecification.Component
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
        .With(FileWithNamespace("lol1.cs", "MyProject"))
        .With(FileWithNamespace("lol2.cs", "MyProject"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasCorrectNamespacesMessage.HasCorrectNamespaces("*MyProject*").Ok());
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
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasCorrectNamespacesMessage.HasCorrectNamespaces($"*MyProject*").Ok());
    }


    [Fact]
    public void ShouldReportErrorWhenMultipleFilesOfSingleProjectAreInWrongNamespaceEvenThoughSomeAreInTheRightOne()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("lol1.cs", "WrongNamespace"))
        .With(FileWithNamespace("lol2.cs", "WrongNamespace"))
        .With(FileWithNamespace("lol3.cs", "MyProject"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasCorrectNamespacesMessage.HasCorrectNamespaces("*MyProject*").Error()
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
        .With(FileWithNamespace("lol1.cs", "WrongNamespace"))
        .With(FileWithNamespace("lol2.cs", "WrongNamespace"))
        .With(FileWithNamespace("lol3.cs", "MyProject1"));
      context.HasProject("MyProject2")
        .WithRootNamespace("MyProject2")
        .With(FileWithNamespace("lol1.cs", "WrongNamespace"))
        .With(FileWithNamespace("lol2.cs", "WrongNamespace"))
        .With(FileWithNamespace("lol3.cs", "MyProject2"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasCorrectNamespacesMessage.HasCorrectNamespaces("*MyProject*").Error()
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
        .With(FileWithNamespace($"Domain{Path.DirectorySeparatorChar}lol4.cs", "MyProject.Domain"))
        .With(FileWithNamespace($"Domain{Path.DirectorySeparatorChar}lol5.cs", "MyProject"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasCorrectNamespacesMessage
        .HasCorrectNamespaces("*MyProject*").Error()
        .ExpectedNamespace("MyProject", "MyProject")
        .ButFoundIncorrectNamespaceFor($"Domain{Path.DirectorySeparatorChar}lol5.cs", "MyProject"));
      context.ReportShouldNotContainText("lol4");
    }

    [Fact]
    public void ShouldReportErrorWhenAFileHasNoNamespace()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(EmptyFile("lol.cs"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasCorrectNamespacesMessage
        .HasCorrectNamespaces("*MyProject*").Error()
        .ExpectedNamespace("MyProject", "MyProject")
        .ButNoNamespaceDeclaredIn("lol.cs"));
    }

    [Fact]
    public void ShouldReportErrorWhenAFileHasMoreThanOneNamespace()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespaces("lol.cs", "MyProject", "MyProject2"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasCorrectNamespaces());

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasCorrectNamespacesMessage
        .HasCorrectNamespaces("*MyProject*").Error()
        .ExpectedNamespace("MyProject", "MyProject")
        .ButHasMultipleNamespaces("lol.cs", "MyProject", "MyProject2"));
    }


    //backlog nested namespaces
    //backlog multiple namespaces per file
    //backlog for all such unmade design decisions, throw exception with reference to github issue
    //        and allow disabling detection of such features via config or commandline parameters
  }
}
