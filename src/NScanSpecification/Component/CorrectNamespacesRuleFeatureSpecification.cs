using Xunit;
using static System.Environment;
using static TddXt.NScan.Specification.Component.DependencyRuleBuilder;

namespace TddXt.NScan.Specification.Component
{
  public class RuleMessage
  {
    public string _returnValue;

    public RuleMessage(string returnValue)
    {
      _returnValue = returnValue;
    }

    public RuleMessage Error()
    {
      return new RuleMessage(_returnValue + "[ERROR]");
    }

    public RuleMessage Ok()
    {
      return new RuleMessage(_returnValue + "[OK]");
    }

    public static RuleMessage HasCorrectNamespacesOk(string projectGlob)
    {
      return new RuleMessage($"{projectGlob} hasCorrectNamespaces: ").Ok();
    }

    public static RuleMessage HasCorrectNamespaces(string projectGlob)
    {
      return new RuleMessage($"{projectGlob} hasCorrectNamespaces: ");
    }

    public RuleMessage WithCorrectNamespaceRuleBroken(
      string projectName, 
      string rootNamespace, 
      string fileName, 
      string actualNamespace)
    {
      return new RuleMessage(_returnValue +
             $"{NewLine}" +
             $"{projectName} has root namespace {rootNamespace}" +
             $" but the file {fileName} has incorrect namespace {actualNamespace}");
    }
  }

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
      context.ReportShouldContainText(RuleMessage.HasCorrectNamespacesOk("*MyProject*")._returnValue);
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
      context.ReportShouldContain(
        RuleMessage.HasCorrectNamespaces("*MyProject*").Error()
          .WithCorrectNamespaceRuleBroken("MyProject", "MyProject", "lol1.cs", "WrongNamespace")
          .WithCorrectNamespaceRuleBroken("MyProject", "MyProject", "lol2.cs", "WrongNamespace"));
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
        RuleMessage.HasCorrectNamespaces("*MyProject*").Error()
          .WithCorrectNamespaceRuleBroken("MyProject1", "MyProject1", "lol1.cs", "WrongNamespace")
          .WithCorrectNamespaceRuleBroken("MyProject1", "MyProject1", "lol2.cs", "WrongNamespace")
          .WithCorrectNamespaceRuleBroken("MyProject2", "MyProject2", "lol1.cs", "WrongNamespace") //bug
          .WithCorrectNamespaceRuleBroken("MyProject2", "MyProject2", "lol2.cs", "WrongNamespace")); //bug
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
      context.ReportShouldContain(RuleMessage
        .HasCorrectNamespaces("*MyProject*").Error()
        .WithCorrectNamespaceRuleBroken("MyProject", "MyProject", "Domain\\lol5.cs", "MyProject"));
      context.ReportShouldNotContainText("lol4");
    }


    //backlog nested namespaces
    //backlog multiple namespaces per file
  }
}
