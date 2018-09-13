using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MyTool.App;
using MyTool.CompositionRoot;
using MyTool.Xml;

namespace MyTool
{
  public class ApplicationContext
  {
    private readonly ISupport _consoleSupport = new ConsoleSupport();
    private readonly List<XmlProject> _xmlProjects = new List<XmlProject>();
    private readonly List<(string, string)> _independentOfRules = new List<(string, string)>();
    private Analysis _analysis;

    public XmlProjectDsl HasProject(string assemblyName)
    {
      var xmlProject = new XmlProject()
      {
        AbsolutePath = AbsolutePathFor(assemblyName),
        PropertyGroups = new List<XmlPropertyGroup>()
        {
          new XmlPropertyGroup()
          {
            AssemblyName = assemblyName
          }
        },
        ItemGroups = new List<XmlItemGroup>()
      };
      _xmlProjects.Add(xmlProject);
      return new XmlProjectDsl(xmlProject);
    }

    public static string AbsolutePathFor(string assemblyName)
    {
      return @"C:\" + assemblyName + ".cs";
    }

    public void AddIndependentOfRule(string dependingAssemblyName, string dependentAssemblyName)
    {
      _independentOfRules.Add((dependingAssemblyName, dependentAssemblyName));
    }

    public void PerformAnalysis()
    {
      _analysis = Analysis.PrepareFor(_xmlProjects, _consoleSupport);

      foreach (var (depending, dependent) in _independentOfRules)
      {
        _analysis.IndependentOfProject(depending, dependent);
      }

      _analysis.Run();
    }

    public void ReportShouldContainText(string expected)
    {
      _analysis.Report.Should().Contain(expected);
      //todo what bout return code?
    }
  }

  public class XmlProjectDsl
  {
    private readonly XmlProject _xmlProject;

    public XmlProjectDsl(XmlProject xmlProject)
    {
      _xmlProject = xmlProject;
    }

    public void WithReferences(params string[] names)
    {
      _xmlProject.ItemGroups = new List<XmlItemGroup>
      {
        new XmlItemGroup() {ProjectReference = ProjectReferencesFrom(names)}
      };
    }

    private static List<XmlProjectReference> ProjectReferencesFrom(string[] names)
    {
      return names.Select(n => new XmlProjectReference()
      {
        Include = ApplicationContext.AbsolutePathFor(n)
      }).ToList();
    }
  }
}