using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using TddXt.NScan.Specification.EndToEnd.AutomationLayer;
using Xunit;
using static TddXt.NScan.Specification.AutomationLayer.XmlSourceCodeFileBuilder;

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
          .With(FileWithNamespace("lol1.cs", "MyProject"))
          .With(FileWithNamespace("lol2.cs", "MyProject"));
        context.Add(DependencyRuleBuilder.RuleRequiring().Project("*MyProject*").HasCorrectNamespaces());

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
          .With(FileWithNamespace("lol1.cs", "WrongNamespace"))
          .With(FileWithNamespace("lol2.cs", "WrongNamespace"))
          .With(FileWithNamespace("lol3.cs", "MyProject"));
        context.Add(DependencyRuleBuilder.RuleRequiring().Project("*MyProject*").HasCorrectNamespaces());

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
          .With(FileWithNamespace("Domain\\lol4.cs", "MyProject.Domain"))
          .With(FileWithNamespace("Domain\\lol5.cs", "MyProject"));
        context.Add(DependencyRuleBuilder.RuleRequiring().Project("*MyProject*").HasCorrectNamespaces());

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
          .With(FileWithNamespace("obj\\lol4.cs", "Trolololo"));
        context.Add(DependencyRuleBuilder.RuleRequiring().Project("*MyProject*").HasCorrectNamespaces());

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
