using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NScan.Adapter.NotifyingSupport;
using NScan.Domain.Root;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos;
using NScanSpecification.Lib.AutomationLayer;

namespace NScanSpecification.Component.AutomationLayer
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

    public void Add(IFullDependencyPathRuleConstructed ruleDefinition)
    {
      _rules.Add(ruleDefinition.Build());
    }
    public void Add(IFullProjectScopedRuleConstructed ruleDefinition)
    {
      _rules.Add(ruleDefinition.Build());
    }
    public void Add(IFullNamespaceBasedRuleConstructed ruleDefinition)
    {
      _rules.Add(ruleDefinition.Build());
    }

    public void PerformAnalysis()
    {
      List<CsharpProjectDto> xmlProjects = _xmlProjects.Select(p => p.BuildCsharpProjectDto()).ToList();
      _analysis = Analysis.PrepareFor(xmlProjects, _consoleSupport);
      _analysis.AddDependencyPathRules(_rules);
      _analysis.Run();
    }

    public void ShouldIndicateSuccess()
    {
      _analysis.OrThrow().ReturnCode.Should().Be(0);
    }

    public void ShouldIndicateFailure()
    {
      _analysis.OrThrow().ReturnCode.Should().Be(-1);
    }

    public void ReportShouldNotContainText(string text)
    {
      _analysis.OrThrow().Report.Should().NotContain(text);
    }

    public void ReportShouldContain(ReportedMessage message)
    {
      _analysis.OrThrow().Report.Should().Contain(message.ToString());
    }
  }
}