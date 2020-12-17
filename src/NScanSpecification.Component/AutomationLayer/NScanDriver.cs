using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NScan.Adapter.NotifyingSupport;
using NScan.Domain;
using NScan.Lib;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NScanSpecification.Lib.AutomationLayer;
using NullableReferenceTypesExtensions;

namespace NScanSpecification.Component.AutomationLayer
{
  public class NScanDriver
  {
    private readonly INScanSupport _consoleSupport = new ConsoleSupport();
    private readonly List<XmlProjectBuilder> _xmlProjects = new List<XmlProjectBuilder>();
    private Analysis? _analysis;
    private readonly List<IAnalysisRule> _rules = new List<IAnalysisRule>();

    public XmlProjectBuilder HasProject(string assemblyName)
    {
      var xmlProjectDsl = XmlProjectBuilder.WithAssemblyName(assemblyName);
      _xmlProjects.Add(xmlProjectDsl);
      return xmlProjectDsl;
    }

    public void Add(IFullDependencyPathRuleConstructed ruleDefinition)
    {
      _rules.Add(new DependencyPathAnalysisRule(ruleDefinition.BuildDependencyPathBasedRule()));
    }
    
    public void Add(IFullProjectScopedRuleConstructed ruleDefinition)
    {
      _rules.Add(new ProjectScopedAnalysisRule(ruleDefinition.BuildProjectScopedRule()));
    }

    public void Add(IFullNamespaceBasedRuleConstructed ruleDefinition)
    {
      _rules.Add(new NamespaceBasedAnalysisRule(ruleDefinition.BuildNamespaceBasedRule()));
    }

    public void PerformAnalysis()
    {
      var xmlProjects = _xmlProjects.Select(p => p.BuildCsharpProjectDto()).ToList();
      _analysis = Analysis.PrepareFor(xmlProjects, _consoleSupport);
      _rules.ForEach(r => r.AddTo(_analysis!));
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

  public class DependencyPathAnalysisRule : IAnalysisRule
  {
    private readonly DependencyPathBasedRuleUnionDto _dto;

    public DependencyPathAnalysisRule(DependencyPathBasedRuleUnionDto dto)
    {
      _dto = dto;
    }

    public void AddTo(Analysis analysis)
    {
      analysis.AddDependencyPathRules(new [] {_dto});
    }
  }

  public class ProjectScopedAnalysisRule : IAnalysisRule
  {
    private readonly ProjectScopedRuleUnionDto _dto;

    public ProjectScopedAnalysisRule(ProjectScopedRuleUnionDto dto)
    {
      _dto = dto;
    }

    public void AddTo(Analysis analysis)
    {
      analysis.AddProjectScopedRules(new [] {_dto});
    }
  }

  public class NamespaceBasedAnalysisRule : IAnalysisRule
  {
    private readonly NamespaceBasedRuleUnionDto _dto;

    public NamespaceBasedAnalysisRule(NamespaceBasedRuleUnionDto dto)
    {
      _dto = dto;
    }

    public void AddTo(Analysis analysis)
    {
      analysis.AddNamespaceBasedRules(new [] {_dto});
    }
  }

  internal interface IAnalysisRule
  {
    void AddTo(Analysis analysis);
  }
}
