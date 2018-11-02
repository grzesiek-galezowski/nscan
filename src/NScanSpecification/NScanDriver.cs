using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GlobExpressions;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using TddXt.NScan.Xml;

namespace TddXt.NScan.Specification
{
  public class NScanDriver
  {
    private readonly INScanSupport _consoleSupport = new ConsoleSupport();
    private readonly List<XmlProject> _xmlProjects = new List<XmlProject>();
    private Analysis _analysis;
    private readonly List<RuleDto> _rules = new List<RuleDto>();

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

    public void AddIndependentOfProjectRule(string dependingAssemblyName, string dependentAssemblyName)
    {
      _rules.Add(new RuleDto()
      {
        DependingPattern = new Glob(dependingAssemblyName),
        DependencyPattern = new Glob(dependentAssemblyName),
        DependencyType = "project",
        RuleName = "independentOf"
      });
    }

    public void AddIndependentOfPackageRule(string projectName, string packageName)
    {
      _rules.Add(new RuleDto()
      {
        DependingPattern = new Glob(projectName),
        DependencyPattern = new Glob(packageName),
        DependencyType = "package",
        RuleName = "independentOf"
      });
    }

    public void AddIndependentOfAssemblyRule(string projectName, string assemblyName)
    {
      _rules.Add(new RuleDto()
      {
        DependingPattern = new Glob(projectName),
        DependencyPattern = new Glob(assemblyName),
        DependencyType = "assembly",
        RuleName = "independentOf"
      });

    }

    public void PerformAnalysis()
    {
      _analysis = Analysis.PrepareFor(_xmlProjects, _consoleSupport);
      _analysis.AddRules(_rules);
      _analysis.Run();
    }

    public void ReportShouldContainText(string expected)
    {
      _analysis.Report.Should().Contain(expected);
    }

    public void ShouldIndicateSuccess()
    {
      _analysis.ReturnCode.Should().Be(0);
    }

    public void ShouldIndicateFailure()
    {
      _analysis.ReturnCode.Should().Be(-1);
    }

  }

  public class XmlProjectDsl
  {
    private readonly XmlProject _xmlProject;

    public XmlProjectDsl(XmlProject xmlProject)
    {
      _xmlProject = xmlProject;
      _xmlProject.ItemGroups = _xmlProject.ItemGroups ?? new List<XmlItemGroup>();
    }

    public void WithReferences(params string[] names)
    {
      _xmlProject.ItemGroups.Add(
        new XmlItemGroup() {ProjectReferences = ProjectReferencesFrom(names)}
      );
    }

    public void WithPackages(params string[] packageNames)
    {
      _xmlProject.ItemGroups.Add(
        new XmlItemGroup() {PackageReferences = PackageReferencesFrom(packageNames)}
      );
    }

    public void WithAssemblyReferences(params string[] assemblyNames)
    {
      _xmlProject.ItemGroups.Add(
        new XmlItemGroup() { AssemblyReferences = AssemblyReferencesFrom(assemblyNames) }
      );
    }

    private List<XmlAssemblyReference> AssemblyReferencesFrom(string[] assemblyNames)
    {
      return assemblyNames.Select(name => new XmlAssemblyReference()
      {
        HintPath = AnyRoot.Root.Any.String(),
        Include = name
      }).ToList();
    }


    private List<XmlPackageReference> PackageReferencesFrom(string[] packageNames)
    {
      return packageNames.Select(name => new XmlPackageReference()
      {
        Include = name,
        Version = "0.0.0"
      }).ToList();
    }

    private static List<XmlProjectReference> ProjectReferencesFrom(string[] names)
    {
      return names.Select(n => new XmlProjectReference()
      {
        Include = NScanDriver.AbsolutePathFor(n)
      }).ToList();
    }


  }
}