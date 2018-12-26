using TddXt.NScan.Specification.Component.AutomationLayer;
using Xunit;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class CorrectNamespacesRuleFeatureSpecification
  {
    [Fact]
    public void ShouldNotReportErrorWhenMultipleFilesOfSingleProjectAreInCorrectNamespace()
    {
      //GIVEN
      using (var context = new NScanE2EDriver())
      {
        context.HasProject("MyProject")
          .WithRootNamespace("MyProject")
          .WithFile("lol1.cs", "MyProject")
          .WithFile("lol2.cs", "MyProject");
        context.Add(DependencyRuleBuilder.Rule().Project("*MyProject*").HasCorrectNamespaces());

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(ReportedMessage.HasCorrectNamespaces("*MyProject*").Ok());
      }
    }

    [Fact]
    public void
      ShouldReportErrorWhenMultipleFilesOfSingleProjectAreInWrongNamespaceEvenThoughSomeAreInTheRightOne()
    {
      //GIVEN
      using (var context = new NScanE2EDriver())
      {
        context.HasProject("MyProject")
          .WithRootNamespace("MyProject")
          .WithFile("lol1.cs", "WrongNamespace")
          .WithFile("lol2.cs", "WrongNamespace")
          .WithFile("lol3.cs", "MyProject");
        context.Add(DependencyRuleBuilder.Rule().Project("*MyProject*").HasCorrectNamespaces());

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(
          ReportedMessage.HasCorrectNamespaces("*MyProject*").Error()
            .ExpectedNamespace("MyProject", "MyProject").ButFoundIncorrectNamespaceFor("lol1.cs", "WrongNamespace")
            .ExpectedNamespace("MyProject", "MyProject").ButFoundIncorrectNamespaceFor("lol2.cs", "WrongNamespace"));
        context.ReportShouldNotContainText("lol3");
      }
    }

    [Fact]
    public void
      ShouldReportErrorWhenMultipleNestedFilesOfSingleProjectAreInWrongNamespaceEvenThoughSomeAreInTheRightOne()
    {
      //GIVEN
      using (var context = new NScanE2EDriver())
      {
        context.HasProject("MyProject")
          .WithRootNamespace("MyProject")
          .WithFile("Domain\\lol4.cs", "MyProject.Domain")
          .WithFile("Domain\\lol5.cs", "MyProject");
        context.Add(DependencyRuleBuilder.Rule().Project("*MyProject*").HasCorrectNamespaces());

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(
          ReportedMessage.HasCorrectNamespaces("*MyProject*").Error()
            .ExpectedNamespace("MyProject", "MyProject")
            .ButFoundIncorrectNamespaceFor("Domain\\lol5.cs", "MyProject"));
        context.ReportShouldNotContainText("lol4");
      }
    }

    [Fact]
    public void
      ShouldIgnoreObjFolderWhenCheckingProjectScopedRule()
    {
      //GIVEN
      using (var context = new NScanE2EDriver())
      {
        context.HasProject("MyProject")
          .WithRootNamespace("MyProject")
          .WithFile("obj\\lol4.cs", "Trolololo");
        context.Add(DependencyRuleBuilder.Rule().Project("*MyProject*").HasCorrectNamespaces());

        //WHEN
        context.PerformAnalysis();

        //THEN
        context.ReportShouldContain(
          ReportedMessage.HasCorrectNamespaces("*MyProject*").Ok());
        context.ReportShouldNotContainText("lol4");
      }
    }

  }
}
