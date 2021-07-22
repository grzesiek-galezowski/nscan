using System.Collections.Generic;
using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace TddXt.NScan.Domain
{
  public class Analysis
  {
    public const int ReturnCodeOk = 0;
    public const int ReturnCodeAnalysisFailed = -1;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;
    private readonly IDependencyAnalysis _dependencyAnalysis;
    private readonly IProjectAnalysis _projectAnalysis;
    private readonly IProjectNamespacesAnalysis  _projectNamespacesAnalysis;

    public Analysis(IAnalysisReportInProgress analysisReportInProgress,
      IDependencyAnalysis dependencyAnalysis,
      IProjectAnalysis projectAnalysis,
      IProjectNamespacesAnalysis projectNamespacesAnalysis)
    {
      _analysisReportInProgress = analysisReportInProgress;
      _dependencyAnalysis = dependencyAnalysis;
      _projectAnalysis = projectAnalysis;
      _projectNamespacesAnalysis = projectNamespacesAnalysis;
    }

    public string Report => _analysisReportInProgress.AsString();
    public int ReturnCode => _analysisReportInProgress.HasViolations() ? -1 : 0; //bug UI implementation leak

    public static Analysis PrepareFor(IEnumerable<CsharpProjectDto> csharpProjectDtos, INScanSupport support)
    {
      return new Analysis(new AnalysisReportInProgress(), 
        DependencyAnalysis.PrepareFor(csharpProjectDtos, support), 
        ProjectAnalysis.PrepareFor(csharpProjectDtos), 
        ProjectNamespacesAnalysis.PrepareFor(csharpProjectDtos));
    }

    public void Run()
    {
      //_solution.PrintDebugInfo();
      _dependencyAnalysis.Perform(_analysisReportInProgress);
      _projectAnalysis.Perform(_analysisReportInProgress);
      _projectNamespacesAnalysis.PerformOn(_analysisReportInProgress);
    }

    public void AddDependencyPathRules(IEnumerable<DependencyPathBasedRuleUnionDto> rules)
    {
      _dependencyAnalysis.Add(rules);
    }

    public void AddProjectScopedRules(IEnumerable<ProjectScopedRuleUnionDto> rules)
    {
      _projectAnalysis.Add(rules);
    }

    public void AddNamespaceBasedRules(IEnumerable<NamespaceBasedRuleUnionDto> rules)
    {
      _projectNamespacesAnalysis.Add(rules);
    }
  }
}
