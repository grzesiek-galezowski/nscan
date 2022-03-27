using NScanSpecification.Component.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.HasTargetFrameworkReportedMessage;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;

namespace NScanSpecification.Component;

public class HasTargetFrameworkRuleFeatureSpecification
{
  public class ProjectsHaveTargetFrameworkSpecification
  {
    [Fact]
    public void ShouldReportSuccessWhenAllProjectsHaveSpecifiedFramework()
    {
      //GIVEN
      var context = new NScanDriver();

      context.HasProject("MyProject")
        .WithTargetFramework(TargetFramework.Default);

      context.Add(
        RuleDemandingThat().Project("*MyProject*")
          .HasTargetFramework(TargetFramework.Default));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasFramework("*MyProject*", TargetFramework.Default).Ok());
    }

    [Fact]
    public void ShouldReportErrorForProjectThatDoesNotHaveSpecifiedFramework()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject").WithTargetFramework(TargetFramework.Default);

      context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.1"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasFramework("*MyProject*", "netstandard2.1").Error()
        .ProjectHasAnotherTargetFramework("MyProject", TargetFramework.Default));
    }

    [Fact]
    public void ShouldReportErrorForMultipleProjectsThatDoNotHaveSpecifiedFramework()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject1").WithTargetFramework(TargetFramework.Default);
      context.HasProject("MyProject2").WithTargetFramework(TargetFramework.Default);

      context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.1"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasFramework("*MyProject*", "netstandard2.1").Error()
        .ProjectHasAnotherTargetFramework("MyProject1", TargetFramework.Default)
        .ProjectHasAnotherTargetFramework("MyProject2", TargetFramework.Default));
    }

    [Fact]
    public void ShouldNotReportErrorForProjectsThatDoNotMatchTheNamePattern()
    {
      //GIVEN
      const string projectAssemblyNameThatDoesNotMatchRuleFilter = "Trolololo";
      var context = new NScanDriver();

      context
        .HasProject(projectAssemblyNameThatDoesNotMatchRuleFilter)
        .WithTargetFramework(TargetFramework.Default);
      context.Add(RuleDemandingThat().Project("*MyProject*").HasTargetFramework("netstandard2.1"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldNotContainText(projectAssemblyNameThatDoesNotMatchRuleFilter);
    }
  }
}