using NScanSpecification.Component.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.HasPropertyReportedMessage;
using static NScanSpecification.Lib.AutomationLayer.RuleBuilder;

namespace NScanSpecification.Component
{
  public class HasPropertyRuleFeatureSpecification
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
          .HasProperty("TargetFramework", TargetFramework.Default));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasProperty("*MyProject*", "TargetFramework", TargetFramework.Default).Ok());
    }

    [Fact]
    public void ShouldReportErrorForProjectThatDoesNotHaveSpecifiedFramework()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject").WithTargetFramework(TargetFramework.Default);
    
      context.Add(RuleDemandingThat().Project("*MyProject*").HasProperty("TargetFramework", "netstandard2.1"));
    
      //WHEN
      context.PerformAnalysis();
    
      //THEN
      context.ReportShouldContain(HasProperty("*MyProject*", "TargetFramework", "netstandard2.1").Error()
        .ProjectHasAnotherPropertyValue("MyProject", "TargetFramework", TargetFramework.Default));
    }

    [Fact]
    public void ShouldReportErrorForProjectThatDoesNotHaveSpecifiedAttribute()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject");
    
      context.Add(RuleDemandingThat().Project("*MyProject*").HasProperty("Trolololo", "lol2"));
    
      //WHEN
      context.PerformAnalysis();
    
      //THEN
      context.ReportShouldContain(HasProperty("*MyProject*", "Trolololo", "lol2").Error()
        .ProjectDoesNotHavePropertySetExplicitly("MyProject", "Trolololo"));
    }
  }
}
