﻿using System.Threading.Tasks;
using NScanSpecification.E2E.AutomationLayer;
using Xunit;
using static NScanSpecification.Lib.AutomationLayer.HasNoCircularUsingsMessage;
using static NScanSpecification.Lib.AutomationLayer.DependencyRuleBuilder;
using static NScanSpecification.Lib.AutomationLayer.SourceCodeFileDtoBuilder;

namespace NScanSpecification.E2E
{
  public class NoCircularNamespaceDependenciesRuleFeatureSpecification
  {
    [Fact]
    public async Task ShouldReportSuccessWhenThereAreNoCircularDependenciesBetweenNamespaces()
    {
      //GIVEN
      using var context = new NScanE2EDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("lol1.cs", "MyProject"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasNoCircularUsings());

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasNoCircularUsings("*MyProject*").Ok());
    }

    [Fact]
    public async Task ShouldReportErrorWhenACycleIsDiscovered()
    {
      //GIVEN
      using var context = new NScanE2EDriver();
      context.HasProject("MyProject")
        .WithRootNamespace("MyProject")
        .With(FileWithNamespace("lol1.cs", "MyProject").Using("MyProject.Util"))
        .With(FileWithNamespace("lol2.cs", "MyProject.Util").Using("MyProject"));
      context.Add(RuleDemandingThat().Project("*MyProject*").HasNoCircularUsings());

      //WHEN
      await context.PerformAnalysis();

      //THEN
      context.ReportShouldContain(HasNoCircularUsings("*MyProject*").Error()
        .CycleFound("MyProject", "MyProject", "MyProject.Util", "MyProject"));
    }


  }
}