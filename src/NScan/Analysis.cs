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

namespace NScan.Domain
{
  public class Analysis
  {
    public const int ReturnCodeOk = 0;
    public const int ReturnCodeAnalysisFailed = -1;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;
    private readonly IDependencyAnalysis _dependencyAnalysis;
    private readonly IProjectAnalysis _projectAnalysis;
    private readonly IProjectNamespacesAnalysis  _projectNamespacesAnalysis;
    private readonly ISolutionForDependencyPathBasedRules _solutionForDependencyPathBasedRules;
    private readonly ISolutionForProjectScopedRules _solutionForProjectScopedRules;
    private readonly ISolutionForNamespaceBasedRules _solutionForNamespaceBasedRules;

    public Analysis(IAnalysisReportInProgress analysisReportInProgress,
      IDependencyAnalysis dependencyAnalysis,
      IProjectAnalysis projectAnalysis,
      IProjectNamespacesAnalysis projectNamespacesAnalysis,
      ISolutionForDependencyPathBasedRules solutionForDependencyPathBasedRules,
      ISolutionForProjectScopedRules solutionForProjectScopedRules,
      ISolutionForNamespaceBasedRules solutionForNamespaceBasedRules)
    {
      _analysisReportInProgress = analysisReportInProgress;
      _dependencyAnalysis = dependencyAnalysis;
      _projectAnalysis = projectAnalysis;
      _projectNamespacesAnalysis = projectNamespacesAnalysis;
      _solutionForDependencyPathBasedRules = solutionForDependencyPathBasedRules;
      _solutionForProjectScopedRules = solutionForProjectScopedRules;
      _solutionForNamespaceBasedRules = solutionForNamespaceBasedRules;
    }

    public string Report => _analysisReportInProgress.AsString();
    public int ReturnCode => _analysisReportInProgress.HasViolations() ? -1 : 0; //bug UI implementation leak

    public static Analysis PrepareFor(IEnumerable<CsharpProjectDto> csharpProjectDtos, INScanSupport support)
    {
      return new Analysis(new AnalysisReportInProgress(), 
        //bug move compositions to specific projects
        new DependencyAnalysis(
          new PathRuleSet(), 
          new DependencyPathRuleFactory(
            new DependencyPathRuleViolationFactory(
              new DependencyPathReportFragmentsFormat()))), 
        new ProjectAnalysis(
          new ProjectScopedRuleSet(), 
          new ProjectScopedRuleFactory(
            new ProjectScopedRuleViolationFactory())), 
        new ProjectNamespacesAnalysis(
          new NamespacesBasedRuleSet(), 
          new NamespaceBasedRuleFactory(
            new NamespaceBasedRuleViolationFactory(
              new NamespaceBasedReportFragmentsFormat()))), 
        new SolutionForDependencyPathRules(new PathCache(
          new DependencyPathFactory()), new DependencyPathBasedRuleTargetFactory(support)
          .CreateDependencyPathRuleTargetsByIds(csharpProjectDtos)), 
        new SolutionForProjectScopedRules(new ProjectScopedRuleTargetFactory(new ProjectScopedRuleViolationFactory())
          .ProjectScopedRuleTargets(csharpProjectDtos)), 
        new SolutionForNamespaceBasedRules(new NamespaceBasedRuleTargetFactory()
          .NamespaceBasedRuleTargets(csharpProjectDtos)));
    }

    public void Run()
    {
      //_solution.PrintDebugInfo();
      _dependencyAnalysis.PerformOn(_solutionForDependencyPathBasedRules, _analysisReportInProgress);
      _projectAnalysis.PerformOn(_solutionForProjectScopedRules, _analysisReportInProgress);
      _projectNamespacesAnalysis.PerformOn(_solutionForNamespaceBasedRules, _analysisReportInProgress);
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