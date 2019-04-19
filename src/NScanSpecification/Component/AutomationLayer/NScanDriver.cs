using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.NotifyingSupport.Adapters;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.NScan.Specification.AutomationLayer;

namespace TddXt.NScan.Specification.Component.AutomationLayer
{
  public class NScanDriver
  {
    private readonly INScanSupport _consoleSupport = new ConsoleSupport();
    private readonly List<XmlProjectBuilder> _xmlProjects = new List<XmlProjectBuilder>();
    private Analysis? _analysis;
    private readonly List<RuleUnionDto> _rules = new List<RuleUnionDto>();

    public XmlProjectBuilder HasProject(string assemblyName)
    {
      var xmlProjectDsl = XmlProjectBuilder.WithAssemblyName(assemblyName);
      _xmlProjects.Add(xmlProjectDsl);
      return xmlProjectDsl;
    }

    public void Add(IFullRuleConstructed ruleDefinition)
    {
      _rules.Add(ruleDefinition.Build());
    }

    public void PerformAnalysis()
    {
      _analysis = Analysis.PrepareFor(_xmlProjects.Select(p => p.Build()).ToList(), _consoleSupport);
      _analysis.AddRules(_rules);
      _analysis.Run();
    }

    public void ShouldIndicateSuccess()
    {
      _analysis!.ReturnCode.Should().Be(0);
    }

    public void ShouldIndicateFailure()
    {
      _analysis!.ReturnCode.Should().Be(-1);
    }

    public void ReportShouldNotContainText(string text)
    {
      _analysis!.Report.Should().NotContain(text);
    }

    public void ReportShouldContain(ReportedMessage message)
    {
      _analysis!.Report.Should().Contain(message.ToString());
    }
  }
}