using System.Collections.Generic;
using FluentAssertions;
using TddXt.NScan.Domain;
using TddXt.NScan.NotifyingSupport.Adapters;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.NScan.ReadingSolution;
using TddXt.NScan.ReadingSolution.Ports;
using TddXt.NScan.Specification.AutomationLayer;

namespace TddXt.NScan.Specification.Component.AutomationLayer
{
  public class NScanDriver
  {
    private readonly INScanSupport _consoleSupport = new ConsoleSupport();
    private readonly List<XmlProject> _xmlProjects = new List<XmlProject>();
    private Analysis _analysis;
    private readonly List<RuleUnionDto> _rules = new List<RuleUnionDto>();

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

    public void ShouldIndicateSuccess()
    {
      _analysis.ReturnCode.Should().Be(0);
    }

    public void ShouldIndicateFailure()
    {
      _analysis.ReturnCode.Should().Be(-1);
    }

    public void ReportShouldNotContainText(string text)
    {
      _analysis.Report.Should().NotContain(text);
    }

    public void ReportShouldContain(ReportedMessage message)
    {
      _analysis.Report.Should().Contain(message.ToString());
    }
  }
}