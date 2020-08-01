using System.Collections.Generic;
using System.Linq;
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
    private readonly ISolution _solution;
    private readonly IDependencyAnalysis _dependencyAnalysis;
    private readonly IProjectAnalysis _projectAnalysis;
    private readonly IProjectNamespacesAnalysis  _projectNamespacesAnalysis;

    public Analysis(
      ISolution solution,
      IAnalysisReportInProgress analysisReportInProgress, 
      IDependencyAnalysis dependencyAnalysis, 
      IProjectAnalysis projectAnalysis, 
      IProjectNamespacesAnalysis projectNamespacesAnalysis)
    {
      _solution = solution;
      _analysisReportInProgress = analysisReportInProgress;
      _dependencyAnalysis = dependencyAnalysis;
      _projectAnalysis = projectAnalysis;
      _projectNamespacesAnalysis = projectNamespacesAnalysis;
    }

    public string Report => _analysisReportInProgress.AsString();
    public int ReturnCode => _analysisReportInProgress.HasViolations() ? -1 : 0;

    public static Analysis PrepareFor(IEnumerable<CsharpProjectDto> csharpProjectDtos, INScanSupport support)
    {
      var csharpWorkspaceModel = new CsharpWorkspaceModel(support, new ProjectScopedRuleViolationFactory());
      var projects = csharpWorkspaceModel.CreateProjectsDictionaryFrom(csharpProjectDtos);
      var namespaceBasedRuleTargets = csharpWorkspaceModel.NamespaceBasedRuleTargets(csharpProjectDtos);
      var projectScopedRuleTargets = csharpWorkspaceModel.ProjectScopedRuleTargets(csharpProjectDtos);

      return new Analysis(new DotNetStandardSolution(projects,
          new PathCache(
            new DependencyPathFactory()), 
          namespaceBasedRuleTargets, 
          projectScopedRuleTargets
          ),
        new AnalysisReportInProgress(), 
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
              new NamespaceBasedReportFragmentsFormat()))));
    }

    public void Run()
    {
      _solution.ResolveAllProjectsReferences();
      _solution.BuildCache();
      //_solution.PrintDebugInfo();
      _dependencyAnalysis.PerformOn(_solution, _analysisReportInProgress);
      _projectAnalysis.PerformOn(_solution, _analysisReportInProgress);
      _projectNamespacesAnalysis.PerformOn(_solution, _analysisReportInProgress);
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