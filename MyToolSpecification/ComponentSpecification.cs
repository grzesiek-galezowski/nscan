using System;
using System.Collections.Generic;
using FluentAssertions;
using MyTool.App;
using MyTool.CompositionRoot;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using Xunit;

namespace MyTool
{
  public class ComponentSpecification
  {
    [Fact]//(Skip = "not implemented yet")]
    public void ShouldDetectDirectRuleBreak()
    {
      //GIVEN
      var context = new ApplicationContext();
      context.HasProject("A");
      context.HasProject("B");

      context.AddIndependentOfRule("A", "B");

      //WHEN

      context.StartAnalysis();


      //THEN
      context.ReportShouldContainLine("Check [A] independent of [B]: [OK]");

    }

    /*[Fact]
    public void ShouldDetectDirectRuleBreak()
    {
      //GIVEN
      var context = new ApplicationContext();
      context.HasProject("A").WithReferences("B", "C", "D");
      context.HasProject("B");
      context.HasProject("C");
      context.HasProject("D");

      context.AddDirectIndependentRule("A", "B");
      
      //WHEN

      context.StartAnaysis();


      //THEN
      context.ReportShouldContain("Expected A to be independent of B, but found otherwise");

    }*/
  }

  public class ApplicationContext
  {
    private readonly Dictionary<ProjectId, IDotNetProject> _projectsById = new Dictionary<ProjectId, IDotNetProject>();
    private readonly ISupport _consoleSupport = new ConsoleSupport();
    private readonly Analysis _analysis;

    public ApplicationContext()
    {
      _analysis = Analysis.Of(_projectsById);
    }

    public void HasProject(string projectId)
    {
      var key = new ProjectId(projectId);
      _projectsById.Add(key, new DotNetStandardProject(Root.Any.String(), key, Array.Empty<ProjectId>(), _consoleSupport));
    }

    public void AddIndependentOfRule(string dependingProject, string dependentProject)
    {
      _analysis.IndependentOfProject(new ProjectId(dependingProject), new ProjectId(dependentProject));
    }

    public void StartAnalysis()
    {
      _analysis.Run();
    }

    public void ReportShouldContainLine(string expected)
    {
      _analysis.Report.Should().Contain(expected + Environment.NewLine);
      //todo what bout return code?
    }
  }
}
