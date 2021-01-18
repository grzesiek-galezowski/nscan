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
        .WithTargetFramework("netcoreapp2.1");

      context.Add(
        RuleDemandingThat().Project("*MyProject*")
          .HasProperty("TargetFramework", "netcoreapp2.1"));

      //WHEN
      context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(
        HasProperty("*MyProject*", "TargetFramework", "netcoreapp2.1").Ok());
    }

    [Fact]
    public void ShouldReportErrorForProjectThatDoesNotHaveSpecifiedFramework()
    {
      //GIVEN
      var context = new NScanDriver();
      context.HasProject("MyProject").WithTargetFramework("netcoreapp3.1");
    
      context.Add(RuleDemandingThat().Project("*MyProject*").HasProperty("TargetFramework", "netstandard2.1"));
    
      //WHEN
      context.PerformAnalysis();
    
      //THEN
      context.ReportShouldContain(HasProperty("*MyProject*", "TargetFramework", "netstandard2.1").Error()
        .ProjectHasAnotherPropertyValue("MyProject", "TargetFramework", "netcoreapp3.1"));
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
