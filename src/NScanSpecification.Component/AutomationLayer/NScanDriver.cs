using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NScan.Adapters.Secondary.NotifyingSupport;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using Core.NullableReferenceTypesExtensions;
using LanguageExt;
using TddXt.NScan.Domain;

namespace NScanSpecification.Component.AutomationLayer;

public class NScanDriver
{
  private readonly INScanSupport _consoleSupport = ConsoleSupport.CreateInstance();
  private Seq<CSharpProjectDtoBuilder> _csharpProjects = Seq<CSharpProjectDtoBuilder>.Empty;
  private Analysis? _analysis;
  private readonly List<IAnalysisRule> _rules = new();

  public CSharpProjectDtoBuilder HasProject(string assemblyName)
  {
    var xmlProjectDsl = CSharpProjectDtoBuilder.WithAssemblyName(assemblyName);
    _csharpProjects = _csharpProjects.Add(xmlProjectDsl);
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
    var xmlProjects = _csharpProjects.Select(p => p.BuildCsharpProjectDto());
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

public class DependencyPathAnalysisRule(DependencyPathBasedRuleUnionDto dto) : IAnalysisRule
{
  public void AddTo(Analysis analysis)
  {
    analysis.AddDependencyPathRules([dto]);
  }
}

public class ProjectScopedAnalysisRule(ProjectScopedRuleUnionDto dto) : IAnalysisRule
{
  public void AddTo(Analysis analysis)
  {
    analysis.AddProjectScopedRules([dto]);
  }
}

public class NamespaceBasedAnalysisRule(NamespaceBasedRuleUnionDto dto) : IAnalysisRule
{
  public void AddTo(Analysis analysis)
  {
    analysis.AddNamespaceBasedRules([dto]);
  }
}

internal interface IAnalysisRule
{
  void AddTo(Analysis analysis);
}
