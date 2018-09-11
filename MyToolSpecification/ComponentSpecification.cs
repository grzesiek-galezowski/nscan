using System;
using System.Collections.Generic;
using FluentAssertions;
using MyTool.App;
using MyTool.CompositionRoot;
using MyTool.Xml;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyTool
{
  public class ComponentSpecification
  {
    [Fact]
    public void ShouldReportAllSatisfiedRules()
    {
      //GIVEN
      var context = new ApplicationContext();
      context.HasProject("A");
      context.HasProject("B");

      context.AddIndependentOfRule("A", "B");

      //WHEN
      context.StartAnalysis();

      //THEN
      context.ReportShouldContainLine("[A] independentOf [B]: [OK]");
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

      context.StartAnalysis();


      //THEN
      context.ReportShouldContain("Expected A to be independent of B, but found otherwise");

    }*/
  }

  public class ApplicationContext
  {
    //bug remove private readonly Dictionary<ProjectId, IDotNetProject> _projectsById = new Dictionary<ProjectId, IDotNetProject>();
    private readonly ISupport _consoleSupport = new ConsoleSupport();
    private readonly List<XmlProject> _xmlProjects = new List<XmlProject>();
    private readonly List<(string, string)> _independentOfRules = new List<(string, string)>();
    private Analysis _analysis;

    public void HasProject(string assemblyName)
    {
      _xmlProjects.Add(new XmlProject()
      {
        AbsolutePath = @"C:\" + assemblyName + ".cs",
        PropertyGroups = new List<XmlPropertyGroup>()
        {
          new XmlPropertyGroup()
          {
            AssemblyName = assemblyName
          }
        },
        ItemGroups = new List<XmlItemGroup>()
      });
    }

    public void AddIndependentOfRule(string dependingAssemblyName, string dependentAssemblyName)
    {
      _independentOfRules.Add((dependingAssemblyName, dependentAssemblyName));
    }

    public void StartAnalysis()
    {
      _analysis = Analysis.PrepareFor(_xmlProjects, _consoleSupport);

      foreach (var (depending, dependent) in _independentOfRules)
      {
        _analysis.IndependentOfProject(depending, dependent);
      }

      _analysis.Run();
    }

    public void ReportShouldContainLine(string expected)
    {
      _analysis.Report.Should().Contain(expected + Environment.NewLine);
      //todo what bout return code?
    }
  }
}
