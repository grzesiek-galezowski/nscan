using NScanSpecification.Component.AutomationLayer;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;
using static NScanSpecification.Lib.AutomationLayer.HasAttributesOnMessage;
using static NScanSpecification.Lib.AutomationLayer.SourceCodeFileDtoBuilder;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;
using static NScanSpecification.Lib.AutomationLayer.ClassDeclarationBuilder;
using static NScanSpecification.Lib.AutomationLayer.MethodDeclarationBuilder;

namespace NScanSpecification.Component
{
  public class HasAttributesOnRuleFeatureSpecification
  {

    [Fact]
    public void ShouldReportSuccessWhenNoFilesInProject()
    {
      //GIVEN
      var context = new NScanDriver();

      context.HasProject("MyProject")
        .WithRootNamespace("MyProject");

      context.Add(RuleDemandingThat().Project("*MyProject*").HasDecoratedMethods("*Specification", "Should*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasMethodsNotDecoratedWithAttribute("*MyProject*", "*Specification", "Should*").Ok());
    }

    [Fact]
    public void ShouldReportSuccessWhenNoClassesInProject()
    {
      //GIVEN
      var context = new NScanDriver();

      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(File("lol.cs"));

      context.Add(RuleDemandingThat().Project("*MyProject*").HasDecoratedMethods("*Specification", "Should*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasMethodsNotDecoratedWithAttribute("*MyProject*", "*Specification", "Should*").Ok());
    }
    
    [Fact]
    public void ShouldReportSuccessWhenNoMethodsInProject()
    {
      //GIVEN
      var context = new NScanDriver();

      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(File("lol.cs").Containing(
          Class("MySpecification")));

      context.Add(RuleDemandingThat().Project("*MyProject*").HasDecoratedMethods("*Specification", "Should*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasMethodsNotDecoratedWithAttribute("*MyProject*", "*Specification", "Should*").Ok());
    }

    [Fact]
    public void ShouldReportSuccessWhenEveryMethodMatchingPatternHasAttributes()
    {
      //GIVEN
      var context = new NScanDriver();

      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(File("lol.cs").Containing(
        Class("MySpecification").With(
          Method("ShouldA")
            .DecoratedWithAttribute(Any.String()),
          Method("ShouldB")
            .DecoratedWithAttribute(Any.String())
          )));

      context.Add(RuleDemandingThat().Project("*MyProject*").HasDecoratedMethods("*Specification", "Should*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasMethodsNotDecoratedWithAttribute("*MyProject*", "*Specification", "Should*").Ok());
    }

    [Fact]
    public void ShouldReportErrorWhenMethodsMatchingPatternAreNotDecoratedWithAttributes()
    {
      //GIVEN
      var context = new NScanDriver();

      const string className = "MySpecification";
      const string methodName1 = "ShouldA";
      const string methodName2 = "ShouldB";
      
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(File("lol.cs").Containing(
          Class(className).With(
            Method(methodName1),
            Method(methodName2))));

      context.Add(RuleDemandingThat().Project("*MyProject*").HasDecoratedMethods("*Specification", "Should*"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasMethodsNotDecoratedWithAttribute("*MyProject*", "*Specification", "Should*")
          .Error()
          .NonCompliantMethodFound(className, methodName1)
          .NonCompliantMethodFound(className, methodName2));
    }

  }
}
