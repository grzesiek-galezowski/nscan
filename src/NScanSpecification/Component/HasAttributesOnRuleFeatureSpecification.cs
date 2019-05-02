using System;
using System.Collections.Generic;
using System.Text;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using Xunit;
using static TddXt.AnyRoot.Root;
using static TddXt.NScan.Specification.AutomationLayer.HasAttributesOnMessage;
using static TddXt.NScan.Specification.AutomationLayer.XmlSourceCodeFileBuilder;
using static TddXt.NScan.Specification.Component.AutomationLayer.DependencyRuleBuilder;
using static TddXt.NScan.Specification.EndToEnd.AutomationLayer.ClassDeclarationBuilder;
using static TddXt.NScan.Specification.EndToEnd.AutomationLayer.MethodDeclarationBuilder;

namespace TddXt.NScan.Specification.Component
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

      context.Add(RuleRequiring().Project("*MyProject*").ToHaveDecoratedMethods("*Specification", "Should*"));

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

      context.Add(RuleRequiring().Project("*MyProject*").ToHaveDecoratedMethods("*Specification", "Should*"));

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
        .With(File("lol.cs").With(
          Class("MySpecification")));

      context.Add(RuleRequiring().Project("*MyProject*").ToHaveDecoratedMethods("*Specification", "Should*"));

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
        .With(File("lol.cs").With(
        Class("MySpecification").With(
          Method("ShouldA")
            .DecoratedWithAttribute(Any.String()),
          Method("ShouldB")
            .DecoratedWithAttribute(Any.String())
          )));

      context.Add(RuleRequiring().Project("*MyProject*").ToHaveDecoratedMethods("*Specification", "Should*"));

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
        .With(File("lol.cs").With(
          Class(className).With(
            Method(methodName1),
            Method(methodName2))));

      context.Add(RuleRequiring().Project("*MyProject*").ToHaveDecoratedMethods("*Specification", "Should*"));

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
