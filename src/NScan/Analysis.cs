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
    private readonly ISolution _solution;
    private readonly IDependencyAnalysis _dependencyAnalysis;
    private readonly IProjectAnalysis _projectAnalysis;
    private readonly IProjectNamespacesAnalysis  _projectNamespacesAnalysis;
    private readonly ISolutionForDependencyPathBasedRules _solutionForDependencyPathBasedRules;
    private readonly ISolutionForProjectScopedRules _solutionForProjectScopedRules;
    private readonly ISolutionForNamespaceBasedRules _solutionForNamespaceBasedRules;

    public Analysis(
      ISolution solution,
      IAnalysisReportInProgress analysisReportInProgress,
      IDependencyAnalysis dependencyAnalysis,
      IProjectAnalysis projectAnalysis,
      IProjectNamespacesAnalysis projectNamespacesAnalysis,
      ISolutionForDependencyPathBasedRules solutionForDependencyPathBasedRules,
      ISolutionForProjectScopedRules solutionForProjectScopedRules,
      ISolutionForNamespaceBasedRules solutionForNamespaceBasedRules)
    {
      _solution = solution;
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
      var dependencyPathBasedRuleTargetFactory = new DependencyPathBasedRuleTargetFactory(support);
      var namespaceBasedRuleTargetFactory = new NamespaceBasedRuleTargetFactory();
      var projectScopedRuleTargetFactory = new ProjectScopedRuleTargetFactory(new ProjectScopedRuleViolationFactory());

      var projectsByIds = dependencyPathBasedRuleTargetFactory.CreateDependencyPathRuleTargetsByIds(csharpProjectDtos);
      var namespaceBasedRuleTargets = namespaceBasedRuleTargetFactory.NamespaceBasedRuleTargets(csharpProjectDtos);
      var projectScopedRuleTargets = projectScopedRuleTargetFactory.ProjectScopedRuleTargets(csharpProjectDtos);

      var pathCache = new PathCache(
        new DependencyPathFactory());
      ISolution solution = new DotNetStandardSolution(projectsByIds
      );
      return new Analysis(solution,
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
              new NamespaceBasedReportFragmentsFormat()))), 
        new SolutionForDependencyPathRules(pathCache, projectsByIds), 
        new SolutionForProjectScopedRules(projectScopedRuleTargets), 
        new SolutionForNamespaceBasedRules(namespaceBasedRuleTargets));
    }

    public void Run()
    {
      _solution.ResolveAllProjectsReferences(); //bug move to dependency rules
      _solutionForDependencyPathBasedRules.BuildDependencyPathCache();
      _solutionForNamespaceBasedRules.BuildNamespacesCache();
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