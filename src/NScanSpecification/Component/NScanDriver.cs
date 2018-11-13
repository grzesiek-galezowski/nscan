using System.Collections.Generic;
using FluentAssertions;
using TddXt.NScan.App;
using TddXt.NScan.Domain;
using TddXt.NScan.RuleInputData;
using TddXt.NScan.Xml;

namespace TddXt.NScan.Specification.Component
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

    public void Add(IFullRuleConstructed ruleDefinition)
    {
      _rules.Add(ruleDefinition.Build());
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
}